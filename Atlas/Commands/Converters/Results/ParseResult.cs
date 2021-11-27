using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

using Atlas.Commands.Interfaces;
using Atlas.Commands.Enums;
using Atlas.Commands.Entities;

namespace Atlas.Commands.Converters.Results
{
    public struct ParseResult : IResult
    {
        public IReadOnlyList<ConverterResult> ArgValues { get; }
        public IReadOnlyList<ConverterResult> ParamValues { get; }

        public CommandError? Error { get; }

        public string ErrorReason { get; }

        public CommandParameterInfo ErrorParameter { get; }

        public bool IsSuccess => !Error.HasValue;

        private ParseResult(IReadOnlyList<ConverterResult> argValues, IReadOnlyList<ConverterResult> paramValues, CommandError? error, string errorReason, CommandParameterInfo errorParamInfo)
        {
            ArgValues = argValues;
            ParamValues = paramValues;
            Error = error;
            ErrorReason = errorReason;
            ErrorParameter = errorParamInfo;
        }

        public static ParseResult FromSuccess(IReadOnlyList<ConverterResult> argValues, IReadOnlyList<ConverterResult> paramValues)
        {
            for (int i = 0; i < argValues.Count; i++)
            {
                if (argValues[i].Values.Count > 1)
                    return new ParseResult(argValues, paramValues, CommandError.MultipleMatches, "Multiple matches found.", null);
            }

            for (int i = 0; i < paramValues.Count; i++)
            {
                if (paramValues[i].Values.Count > 1)
                    return new ParseResult(argValues, paramValues, CommandError.MultipleMatches, "Multiple matches found.", null);
            }

            return new ParseResult(argValues, paramValues, null, null, null);
        }
        public static ParseResult FromSuccess(IReadOnlyList<ConverterValue> argValues, IReadOnlyList<ConverterValue> paramValues)
        {
            var argList = new ConverterResult[argValues.Count];

            for (int i = 0; i < argValues.Count; i++)
                argList[i] = ConverterResult.FromSuccess(argValues[i]);

            ConverterResult[] paramList = null;

            if (paramValues != null)
            {
                paramList = new ConverterResult[paramValues.Count];

                for (int i = 0; i < paramValues.Count; i++)
                    paramList[i] = ConverterResult.FromSuccess(paramValues[i]);
            }

            return new ParseResult(argList, paramList, null, null, null);
        }

        public static ParseResult FromError(CommandError error, string reason)
            => new ParseResult(null, null, error, reason, null);

        public static ParseResult FromError(CommandError error, string reason, CommandParameterInfo parameterInfo)
            => new ParseResult(null, null, error, reason, parameterInfo);

        public static ParseResult FromError(Exception ex)
            => FromError(CommandError.Exception, ex.Message);

        public static ParseResult FromError(IResult result)
            => new ParseResult(null, null, result.Error, result.ErrorReason, null);

        public static ParseResult FromError(IResult result, CommandParameterInfo parameterInfo)
            => new ParseResult(null, null, result.Error, result.ErrorReason, parameterInfo);

        public override string ToString() => IsSuccess ? "Success" : $"{Error}: {ErrorReason}";
    }
}
