using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

using Atlas.Commands.Converters.Results;
using Atlas.Commands.Enums;

namespace Atlas.Commands.Entities
{
    internal static class CommandParser
    {
        private enum ParserPart
        {
            None,
            Parameter,
            QuotedParameter
        }

        public static ParseResult ParseArgs(CommandInfo command, CommandContext context, bool ignoreExtraArgs, string input, int startPos, IReadOnlyDictionary<char, char> aliasMap)
        {
            CommandParameterInfo curParam = null;
            StringBuilder argBuilder = new StringBuilder(input.Length);
            int endPos = input.Length;
            var curPart = ParserPart.None;
            int lastArgEndPos = int.MinValue;
            var argList = ImmutableArray.CreateBuilder<ConverterResult>();
            var paramList = ImmutableArray.CreateBuilder<ConverterResult>();
            bool isEscaping = false;
            char c, matchQuote = '\0';

            bool IsOpenQuote(IReadOnlyDictionary<char, char> dict, char ch)
            {
                if (dict.Count != 0)
                    return dict.ContainsKey(ch);
                return c == '\"';
            }

            char GetMatch(IReadOnlyDictionary<char, char> dict, char ch)
            {
                if (dict.Count != 0 && dict.TryGetValue(c, out var value))
                    return value;
                return '\"';
            }

            for (int curPos = startPos; curPos <= endPos; curPos++)
            {
                if (curPos < endPos)
                    c = input[curPos];
                else
                    c = '\0';

                if (curParam != null && curParam.IsRemainder && curPos != endPos)
                {
                    argBuilder.Append(c);
                    continue;
                }

                if (isEscaping)
                {
                    if (curPos != endPos)
                    {
                        if (c != matchQuote)
                        {
                            argBuilder.Append('\\');
                        }

                        argBuilder.Append(c);
                        isEscaping = false;
                        continue;
                    }
                }

                if (c == '\\' && (curParam == null || !curParam.IsRemainder))
                {
                    isEscaping = true;
                    continue;
                }

                if (curPart == ParserPart.None)
                {
                    if (char.IsWhiteSpace(c) || curPos == endPos)
                        continue; 
                    else if (curPos == lastArgEndPos)
                        return ParseResult.FromError(CommandError.ParseFailed, "There must be at least one character of whitespace between arguments.");
                    else
                    {
                        if (curParam == null)
                            curParam = command.Parameters.Count > argList.Count ? command.Parameters[argList.Count] : null;

                        if (curParam != null && curParam.IsRemainder)
                        {
                            argBuilder.Append(c);
                            continue;
                        }

                        if (IsOpenQuote(aliasMap, c))
                        {
                            curPart = ParserPart.QuotedParameter;
                            matchQuote = GetMatch(aliasMap, c);
                            continue;
                        }
                        curPart = ParserPart.Parameter;
                    }
                }

                string argString = null;

                if (curPart == ParserPart.Parameter)
                {
                    if (curPos == endPos || char.IsWhiteSpace(c))
                    {
                        argString = argBuilder.ToString();
                        lastArgEndPos = curPos;
                    }
                    else
                        argBuilder.Append(c);
                }
                else if (curPart == ParserPart.QuotedParameter)
                {
                    if (c == matchQuote)
                    {
                        argString = argBuilder.ToString(); 
                        lastArgEndPos = curPos + 1;
                    }
                    else
                        argBuilder.Append(c);
                }

                if (argString != null)
                {
                    if (curParam == null)
                    {
                        if (command.IgnoreExtraArgs)
                            break;
                        else
                            return ParseResult.FromError(CommandError.BadArgCount, "The input text has too many parameters.");
                    }

                    var typeReaderResult = curParam.Parse(context, argString);

                    if (!typeReaderResult.IsSuccess && typeReaderResult.Error != CommandError.MultipleMatches)
                        return ParseResult.FromError(typeReaderResult, curParam);

                    if (curParam.IsMultiple)
                    {
                        paramList.Add(typeReaderResult);

                        curPart = ParserPart.None;
                    }
                    else
                    {
                        argList.Add(typeReaderResult);

                        curParam = null;
                        curPart = ParserPart.None;
                    }

                    argBuilder.Clear();
                }
            }

            if (curParam != null && curParam.IsRemainder)
            {
                var typeReaderResult = curParam.Parse(context, argBuilder.ToString());

                if (!typeReaderResult.IsSuccess)
                    return ParseResult.FromError(typeReaderResult, curParam);

                argList.Add(typeReaderResult);
            }

            if (isEscaping)
                return ParseResult.FromError(CommandError.ParseFailed, "Input text may not end on an incomplete escape.");
            if (curPart == ParserPart.QuotedParameter)
                return ParseResult.FromError(CommandError.ParseFailed, "A quoted parameter is incomplete.");

            for (int i = argList.Count; i < command.Parameters.Count; i++)
            {
                var param = command.Parameters[i];

                if (param.IsMultiple)
                    continue;

                if (!param.IsOptional)
                    return ParseResult.FromError(CommandError.BadArgCount, "The input text has too few parameters.");

                argList.Add(ConverterResult.FromSuccess(param.DefaultValue));
            }

            return ParseResult.FromSuccess(argList.ToImmutableList(), paramList.ToImmutableList());
        }
    }
}