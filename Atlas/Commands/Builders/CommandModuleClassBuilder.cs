using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

using Atlas.Commands.Interfaces;
using Atlas.Commands.Entities;
using Atlas.Commands.Utilities;
using Atlas.Commands.Converters;
using Atlas.Commands.Converters.Results;
using Atlas.Commands.Attributes;

namespace Atlas.Commands.Builders
{
    internal static class CommandModuleClassBuilder
    {
        private static readonly TypeInfo ModuleTypeInfo = typeof(ICommandModule).GetTypeInfo();

        public static IReadOnlyList<TypeInfo> Search(Assembly assembly, CommandManager manager)
        {
            var result = new List<TypeInfo>();

            foreach (var typeInfo in assembly.DefinedTypes)
            {
                if (typeInfo.IsPublic || typeInfo.IsNestedPublic)
                {
                    if (IsValidModuleDefinition(typeInfo))
                    {
                        result.Add(typeInfo);
                    }
                }
            }

            return result;
        }


        public static Dictionary<Type, CommandModuleInfo> Build(CommandManager manager, params TypeInfo[] validTypes) => Build(validTypes, manager);
        public static Dictionary<Type, CommandModuleInfo> Build(IEnumerable<TypeInfo> validTypes, CommandManager manager)
        {
            var topLevelGroups = validTypes.Where(x => x.DeclaringType == null || !IsValidModuleDefinition(x.DeclaringType.GetTypeInfo()));

            var builtTypes = new List<TypeInfo>();

            var result = new Dictionary<Type, CommandModuleInfo>();

            foreach (var typeInfo in topLevelGroups)
            {
                if (result.ContainsKey(typeInfo.AsType()))
                    continue;

                var module = new CommandModuleInfoBuilder(manager, null);

                BuildModule(module, typeInfo, manager);
                BuildSubTypes(module, typeInfo.DeclaredNestedTypes, builtTypes, manager);
                builtTypes.Add(typeInfo);

                result[typeInfo.AsType()] = module.Build(manager);
            }

            return result;
        }

        private static void BuildSubTypes(CommandModuleInfoBuilder builder, IEnumerable<TypeInfo> subTypes, List<TypeInfo> builtTypes, CommandManager manager)
        {
            foreach (var typeInfo in subTypes)
            {
                if (!IsValidModuleDefinition(typeInfo))
                    continue;

                if (builtTypes.Contains(typeInfo))
                    continue;

                builder.AddModule((module) =>
                {
                    BuildModule(module, typeInfo, manager);
                    BuildSubTypes(module, typeInfo.DeclaredNestedTypes, builtTypes, manager);
                });

                builtTypes.Add(typeInfo);
            }
        }

        private static void BuildModule(CommandModuleInfoBuilder builder, TypeInfo typeInfo, CommandManager manager)
        {
            var attributes = typeInfo.GetCustomAttributes();
            builder.TypeInfo = typeInfo;

            foreach (var attribute in attributes)
            {
                switch (attribute)
                {
                    case NameAttribute name:
                        builder.Name = name.Text;
                        break;
                    case SummaryAttribute summary:
                        builder.Summary = summary.Text;
                        break;
                    case AliasAttribute alias:
                        builder.AddAliases(alias.Aliases);
                        break;
                    default:
                        builder.AddAttributes(attribute);
                        break;
                }
            }

            if (builder.Aliases.Count == 0)
                builder.AddAliases("");
            if (builder.Name == null)
                builder.Name = typeInfo.Name;

            var validCommands = typeInfo.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Where(IsValidCommandDefinition);

            foreach (var method in validCommands)
            {
                builder.AddCommand((command) =>
                {
                    BuildCommand(command, typeInfo, method, manager);
                });
            }
        }

        private static void BuildCommand(CommandInfoBuilder builder, TypeInfo typeInfo, MethodInfo method, CommandManager manager)
        {
            var attributes = method.GetCustomAttributes();

            foreach (var attribute in attributes)
            {
                switch (attribute)
                {
                    case CommandAttribute command:
                        builder.AddAliases(command.Text);
                        builder.Name = builder.Name ?? command.Text;
                        builder.IgnoreExtraArgs = command.IgnoreExtraArgs ?? manager.Config.IgnoreExtraArgs;
                        break;
                    case NameAttribute name:
                        builder.Name = name.Text;
                        break;
                    case PriorityAttribute priority:
                        builder.Priority = priority.Priority;
                        break;
                    case SummaryAttribute summary:
                        builder.Summary = summary.Text;
                        break;
                    case AliasAttribute alias:
                        builder.AddAliases(alias.Aliases);
                        break;
                    case TypeAttribute typeattr:
                        builder.WithType(typeattr.Types);
                        break;
                    default:
                        builder.AddAttributes(attribute);
                        break;
                }
            }

            if (builder.Name == null)
                builder.Name = method.Name;

            var parameters = method.GetParameters();
            int pos = 0, count = parameters.Length;

            foreach (var paramInfo in parameters)
            {
                builder.AddParameter((parameter) =>
                {
                    BuildParameter(parameter, paramInfo, pos++, count, manager);
                });
            }

            var createInstance = ReflectionUtils.CreateBuilder<ICommandModule>(typeInfo, manager);

            IResult ExecuteCallback(CommandContext context, object[] args, CommandInfo cmd)
            {
                var instance = createInstance();

                instance.SetContext(context);

                try
                {
                    var result = method.Invoke(instance, args) as IResult;

                    return ExecuteResult.FromSuccess();
                }
                finally
                {
                    (instance as IDisposable)?.Dispose();
                }
            }

            builder.Callback = ExecuteCallback;
        }

        private static void BuildParameter(CommandParameterInfoBuilder builder, ParameterInfo paramInfo, int position, int count, CommandManager manager)
        {
            var attributes = paramInfo.GetCustomAttributes();
            var paramType = paramInfo.ParameterType;

            builder.Name = paramInfo.Name;
            builder.IsOptional = paramInfo.IsOptional;
            builder.DefaultValue = paramInfo.HasDefaultValue ? paramInfo.DefaultValue : null;

            foreach (var attribute in attributes)
            {
                switch (attribute)
                {
                    case SummaryAttribute summary:
                        builder.Summary = summary.Text;
                        break;
                    case ParamArrayAttribute _:
                        builder.IsMultiple = true;
                        paramType = paramType.GetElementType();
                        break;
                    case NameAttribute name:
                        builder.Name = name.Text;
                        break;
                    case RemainderAttribute _:
                        if (position != count - 1)
                            throw new InvalidOperationException($"Remainder parameters must be the last parameter in a command. Parameter: {paramInfo.Name} in {paramInfo.Member.DeclaringType.Name}.{paramInfo.Member.Name}");

                        builder.IsRemainder = true;
                        break;
                    default:
                        builder.AddAttributes(attribute);
                        break;
                }
            }

            builder.ParameterType = paramType;

            if (builder.Converter == null)
            {
                builder.Converter = manager.GetDefaultConverter(paramType)
                    ?? manager.GetConverters(paramType)?.FirstOrDefault().Value;
            }
        }

        internal static Converter GetTypeReader(CommandManager manager, Type paramType, Type typeReaderType)
        {
            var converters = manager.GetConverters(paramType);

            Converter converter = null;

            if (converters != null)
            {
                if (converters.TryGetValue(typeReaderType, out converter))
                    return converter;
            }

            converter = ReflectionUtils.CreateObject<Converter>(typeReaderType.GetTypeInfo(), manager);

            manager.AddConverter(paramType, converter, false);

            return converter;
        }

        private static bool IsValidModuleDefinition(TypeInfo typeInfo)
        {
            return ModuleTypeInfo.IsAssignableFrom(typeInfo) &&
                   !typeInfo.IsAbstract &&
                   !typeInfo.ContainsGenericParameters;
        }

        private static bool IsValidCommandDefinition(MethodInfo methodInfo)
        {
            return methodInfo.IsDefined(typeof(CommandAttribute)) &&
                   (methodInfo.ReturnType == typeof(void) || methodInfo.ReturnType == typeof(IResult) &&
                   !methodInfo.IsStatic &&
                   !methodInfo.IsGenericMethod);
        }
    }
}
