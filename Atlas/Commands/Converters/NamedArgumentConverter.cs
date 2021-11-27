using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

using Atlas.Commands.Utilities;
using Atlas.Commands.Entities;
using Atlas.Commands.Enums;
using Atlas.Commands.Converters.Results;

namespace Atlas.Commands.Converters
{
    internal sealed class NamedArgumentConverter<T> : Converter
        where T : class, new()
    {
        private static readonly IReadOnlyDictionary<string, PropertyInfo> _tProps = typeof(T).GetTypeInfo().DeclaredProperties
            .Where(p => p.SetMethod != null && p.SetMethod.IsPublic && !p.SetMethod.IsStatic)
            .ToImmutableDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase);

        private readonly CommandManager _commands;

        public NamedArgumentConverter(CommandManager commands)
        {
            _commands = commands;
        }

        public override ConverterResult Convert(CommandContext context, string input)
        {
            var result = new T();
            var state = ReadState.LookingForParameter;
            int beginRead = 0, currentRead = 0;

            while (state != ReadState.End)
            {
                try
                {
                    var prop = Read(out var arg);
                    var propVal = ReadArgument(prop, arg);

                    if (propVal != null)
                        prop.SetMethod.Invoke(result, new[] { propVal });
                    else
                        return ConverterResult.FromError(CommandError.ParseFailed, $"Could not parse the argument for the parameter '{prop.Name}' as type '{prop.PropertyType}'.");
                }
                catch (Exception ex)
                {
                    return ConverterResult.FromError(ex);
                }
            }

            return ConverterResult.FromSuccess(result);

            PropertyInfo Read(out string arg)
            {
                string currentParam = null;
                char match = '\0';

                for (; currentRead < input.Length; currentRead++)
                {
                    var currentChar = input[currentRead];
                    switch (state)
                    {
                        case ReadState.LookingForParameter:
                            if (Char.IsWhiteSpace(currentChar))
                                continue;
                            else
                            {
                                beginRead = currentRead;
                                state = ReadState.InParameter;
                            }
                            break;
                        case ReadState.InParameter:
                            if (currentChar != ':')
                                continue;
                            else
                            {
                                currentParam = input.Substring(beginRead, currentRead - beginRead);
                                state = ReadState.LookingForArgument;
                            }
                            break;
                        case ReadState.LookingForArgument:
                            if (Char.IsWhiteSpace(currentChar))
                                continue;
                            else
                            {
                                beginRead = currentRead;
                                state = (QuotationAliasUtils.DefaultAliasMap.TryGetValue(currentChar, out match))
                                    ? ReadState.InQuotedArgument
                                    : ReadState.InArgument;
                            }
                            break;
                        case ReadState.InArgument:
                            if (!Char.IsWhiteSpace(currentChar))
                                continue;
                            else
                                return GetPropAndValue(out arg);
                        case ReadState.InQuotedArgument:
                            if (currentChar != match)
                                continue;
                            else
                                return GetPropAndValue(out arg);
                    }
                }

                if (currentParam == null)
                    throw new InvalidOperationException("No parameter name was read.");

                return GetPropAndValue(out arg);

                PropertyInfo GetPropAndValue(out string argv)
                {
                    bool quoted = state == ReadState.InQuotedArgument;
                    state = (currentRead == (quoted ? input.Length - 1 : input.Length))
                        ? ReadState.End
                        : ReadState.LookingForParameter;

                    if (quoted)
                    {
                        argv = input.Substring(beginRead + 1, currentRead - beginRead - 1).Trim();
                        currentRead++;
                    }
                    else
                        argv = input.Substring(beginRead, currentRead - beginRead);

                    return _tProps[currentParam];
                }
            }

            object ReadArgument(PropertyInfo prop, string arg)
            {
                var elemType = prop.PropertyType;
                bool isCollection = false;

                if (elemType.GetTypeInfo().IsGenericType && elemType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                {
                    elemType = prop.PropertyType.GenericTypeArguments[0];
                    isCollection = true;
                }

                var reader = _commands.GetDefaultConverter(elemType) ?? _commands.GetConverters(elemType).FirstOrDefault().Value;

                if (reader != null)
                {
                    if (isCollection)
                    {
                        var method = _readMultipleMethod.MakeGenericMethod(elemType);

                        return (IEnumerable)method.Invoke(null, new object[] { reader, context, arg.Split(',') });
                    }
                    else
                        return ReadSingle(reader, context, arg);
                }
                return null;
            }
        }

        private static object ReadSingle(Converter converter, CommandContext context, string arg)
        {
            var readResult = converter.Convert(context, arg);

            return (readResult.IsSuccess)
                ? readResult.BestMatch
                : null;
        }

        private static IEnumerable ReadMultiple<TObj>(Converter converter, CommandContext context, IEnumerable<string> args, IServiceProvider services)
        {
            var objs = new List<TObj>();

            foreach (var arg in args)
            {
                var read = ReadSingle(converter, context, arg.Trim());

                if (read != null)
                    objs.Add((TObj)read);
            }

            return objs.ToImmutableArray();
        }

        private static readonly MethodInfo _readMultipleMethod = typeof(NamedArgumentConverter<T>)
            .GetTypeInfo()
            .DeclaredMethods
            .Single(m => m.IsPrivate && m.IsStatic && m.Name == nameof(ReadMultiple));

        private enum ReadState
        {
            LookingForParameter,
            InParameter,
            LookingForArgument,
            InArgument,
            InQuotedArgument,
            End
        }
    }
}
