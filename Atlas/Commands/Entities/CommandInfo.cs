using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

using Atlas.Commands.Builders;
using Atlas.Commands.Utilities;
using Atlas.Commands.Interfaces;
using Atlas.Commands.Converters.Results;
using Atlas.Commands.Extensions;
using Atlas.Commands.Enums;

namespace Atlas.Commands.Entities
{
    public class CommandInfo
    {
        private static readonly MethodInfo _convertParamsMethod = typeof(CommandInfo).GetTypeInfo().GetDeclaredMethod(nameof(ConvertParamsList));
        private static readonly ConcurrentDictionary<Type, Func<IEnumerable<object>, object>> _arrayConverters = new ConcurrentDictionary<Type, Func<IEnumerable<object>, object>>();

        private readonly CommandManager _commandManager;
        private readonly Func<CommandContext, object[], CommandInfo, IResult> _action;

        public CommandModuleInfo Module { get; }

        public string Name { get; }

        public string Summary { get; }

        public string Remarks { get; }

        public int Priority { get; }

        public bool HasVarArgs { get; }

        public bool IgnoreExtraArgs { get; }

        public HashSet<CommandType> Types { get; }

        public IReadOnlyList<string> Aliases { get; }

        public IReadOnlyList<CommandParameterInfo> Parameters { get; }

        public IReadOnlyList<Attribute> Attributes { get; }

        internal CommandInfo(CommandInfoBuilder builder, CommandModuleInfo module, CommandManager service)
        {
            Module = module;

            Types = builder.Types ?? new HashSet<CommandType>() { CommandType.RemoteAdmin, CommandType.ServerConsole };
            Name = builder.Name;
            Summary = builder.Summary;
            Remarks = builder.Remarks;
            Priority = builder.Priority;

            Aliases = module.Aliases
                .Permutate(builder.Aliases, (first, second) =>
                {
                    if (first == "")
                        return second;
                    else if (second == "")
                        return first;
                    else
                        return first + service.Config.SeparatorChar + second;
                })
                .Select(x => service.Config.CaseSensitiveCommands ? x : x.ToLowerInvariant())
                .ToImmutableArray();

            Attributes = builder.Attributes.ToImmutableArray();

            Parameters = builder.Parameters.Select(x => x.Build(this)).ToImmutableList();
            HasVarArgs = builder.Parameters.Count > 0 && builder.Parameters[builder.Parameters.Count - 1].IsMultiple;
            IgnoreExtraArgs = builder.IgnoreExtraArgs;

            _action = builder.Callback;
            _commandManager = service;
        }

        public ParseResult Parse(CommandContext context, int startIndex, SearchResult searchResult)
        {
            if (!searchResult.IsSuccess)
                return ParseResult.FromError(searchResult);

            string input = searchResult.Text.Substring(startIndex);

            return CommandParser.ParseArgs(this, context, _commandManager.Config.IgnoreExtraArgs, input, 0, QuotationAliasUtils.DefaultAliasMap);
        }

        public IResult Execute(CommandContext context, ParseResult parseResult)
        {
            if (!parseResult.IsSuccess)
                return ExecuteResult.FromError(parseResult);

            var argList = new object[parseResult.ArgValues.Count];

            for (int i = 0; i < parseResult.ArgValues.Count; i++)
            {
                if (!parseResult.ArgValues[i].IsSuccess)
                    return ExecuteResult.FromError(parseResult.ArgValues[i]);

                argList[i] = parseResult.ArgValues[i].Values.First().Value;
            }

            var paramList = new object[parseResult.ParamValues.Count];

            for (int i = 0; i < parseResult.ParamValues.Count; i++)
            {
                if (!parseResult.ParamValues[i].IsSuccess)
                    return ExecuteResult.FromError(parseResult.ParamValues[i]);

                paramList[i] = parseResult.ParamValues[i].Values.First().Value;
            }

            return Execute(context, argList, paramList);
        }
        public IResult Execute(CommandContext context, IEnumerable<object> argList, IEnumerable<object> paramList)
        {
            try
            {
                object[] args = GenerateArgs(argList, paramList);

                ExecuteInternal(context, args);

                return ExecuteResult.FromSuccess();
            }
            catch (Exception ex)
            {
                return ExecuteResult.FromError(ex);
            }
        }

        private IResult ExecuteInternal(CommandContext context, object[] args)
        {
            try
            {
                IResult result = _action(context, args, this);

                if (result == null)
                {
                    return ExecuteResult.FromError(Enums.CommandError.Unsuccessful, "Unknown error.");
                }
                else
                {
                    if (result is ExecuteResult execute)
                        return execute;
                }

                return ExecuteResult.FromSuccess();
            }
            catch (Exception ex)
            {
                return ExecuteResult.FromError(ex);
            }
        }

        private object[] GenerateArgs(IEnumerable<object> argList, IEnumerable<object> paramsList)
        {
            int argCount = Parameters.Count;
            var array = new object[Parameters.Count];

            if (HasVarArgs)
                argCount--;

            int i = 0;

            foreach (object arg in argList)
            {
                if (i == argCount)
                    throw new InvalidOperationException("Command was invoked with too many parameters.");
                array[i++] = arg;
            }

            if (i < argCount)
                throw new InvalidOperationException("Command was invoked with too few parameters.");

            if (HasVarArgs)
            {
                var func = _arrayConverters.GetOrAdd(Parameters[Parameters.Count - 1].Type, t =>
                {
                    var method = _convertParamsMethod.MakeGenericMethod(t);
                    return (Func<IEnumerable<object>, object>)method.CreateDelegate(typeof(Func<IEnumerable<object>, object>));
                });

                array[i] = func(paramsList);
            }

            return array;
        }

        private static T[] ConvertParamsList<T>(IEnumerable<object> paramsList)
            => paramsList.Cast<T>().ToArray();
    }
}