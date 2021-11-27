using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Atlas.Commands.Interfaces;
using Atlas.Commands.Entities;
using Atlas.Commands.Enums;

namespace Atlas.Commands.Converters.Results
{
    public struct ConverterResult : IResult
    {
        public IReadOnlyCollection<ConverterValue> Values { get; }

        public CommandError? Error { get; }

        public string ErrorReason { get; }

        public bool IsSuccess => !Error.HasValue;

        public object BestMatch => IsSuccess
            ? (Values.Count == 1 ? Values.Single().Value : Values.OrderByDescending(v => v.Score).First().Value)
            : throw new InvalidOperationException("ConverterResult was not successful.");

        private ConverterResult(IReadOnlyCollection<ConverterValue> values, CommandError? error, string errorReason)
        {
            Values = values;
            Error = error;
            ErrorReason = errorReason;
        }

        public static ConverterResult FromSuccess(object value)
            => new ConverterResult(ImmutableArray.Create(new ConverterValue(value, 1.0f)), null, null);

        public static ConverterResult FromSuccess(ConverterValue value)
            => new ConverterResult(ImmutableArray.Create(value), null, null);

        public static ConverterResult FromSuccess(IReadOnlyCollection<ConverterValue> values)
            => new ConverterResult(values, null, null);

        public static ConverterResult FromError(CommandError error, string reason)
            => new ConverterResult(null, error, reason);

        public static ConverterResult FromError(Exception ex)
            => FromError(CommandError.Exception, ex.Message);

        public static ConverterResult FromError(IResult result)
            => new ConverterResult(null, result.Error, result.ErrorReason);

        public override string ToString() => IsSuccess ? "Success" : $"{Error}: {ErrorReason}";
    }
}
