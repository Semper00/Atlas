using System;

using Atlas.Commands.Converters.Results;
using Atlas.Commands.Entities;
using Atlas.Commands.Enums;

namespace Atlas.Commands.Converters
{
    internal static class PrimitiveTypeConverter
    {
        public static Converter Create(Type type)
        {
            type = typeof(PrimitiveTypeConverter<>).MakeGenericType(type);

            return Activator.CreateInstance(type) as Converter;
        }
    }

    internal class PrimitiveTypeConverter<T> : Converter
    {
        private readonly TryParseDelegate<T> _tryParse;
        private readonly float _score;

        public PrimitiveTypeConverter()
            : this(PrimitiveConverters.Get<T>(), 1)
        { }

        public PrimitiveTypeConverter(TryParseDelegate<T> tryParse, float score)
        {
            if (score < 0 || score > 1)
                throw new ArgumentOutOfRangeException(nameof(score), score, "Scores must be within the range [0, 1].");

            _tryParse = tryParse;
            _score = score;
        }

        public override ConverterResult Convert(CommandContext context, string input)
        {
            if (_tryParse(input, out T value))
                return ConverterResult.FromSuccess(new ConverterValue(value, _score));

            return ConverterResult.FromError(CommandError.ParseFailed, $"Failed to parse {typeof(T).Name}.");
        }
    }
}
