using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

using Atlas.Commands.Enums;
using Atlas.Commands.Converters.Results;

namespace Atlas.Commands.Converters
{
    internal static class EnumConverter
    {
        public static Converter GetReader(Type type)
        {
            Type baseType = Enum.GetUnderlyingType(type);

            var constructor = typeof(EnumConverter<>).MakeGenericType(baseType).GetTypeInfo().DeclaredConstructors.First();

            return (Converter)constructor.Invoke(new object[] { type, PrimitiveConverters.Get(baseType) });
        }
    }

    internal class EnumConverter<T> : Converter
    {
        private readonly IReadOnlyDictionary<string, object> _enumsByName;
        private readonly IReadOnlyDictionary<T, object> _enumsByValue;
        private readonly Type _enumType;
        private readonly TryParseDelegate<T> _tryParse;

        public EnumConverter(Type type, TryParseDelegate<T> parser)
        {
            _enumType = type;
            _tryParse = parser;

            var byNameBuilder = ImmutableDictionary.CreateBuilder<string, object>();
            var byValueBuilder = ImmutableDictionary.CreateBuilder<T, object>();

            foreach (var v in Enum.GetNames(_enumType))
            {
                var parsedValue = Enum.Parse(_enumType, v);
                byNameBuilder.Add(v.ToLower(), parsedValue);
                if (!byValueBuilder.ContainsKey((T)parsedValue))
                    byValueBuilder.Add((T)parsedValue, parsedValue);
            }

            _enumsByName = byNameBuilder.ToImmutable();
            _enumsByValue = byValueBuilder.ToImmutable();
        }

        /// <inheritdoc />
        public override ConverterResult Convert(CommandContext context, string input)
        {
            object enumValue;

            if (_tryParse(input, out T baseValue))
            {
                if (_enumsByValue.TryGetValue(baseValue, out enumValue))
                    return ConverterResult.FromSuccess(enumValue);
                else
                    return ConverterResult.FromError(CommandError.ParseFailed, $"Value is not a {_enumType.Name}.");
            }
            else
            {
                if (_enumsByName.TryGetValue(input.ToLower(), out enumValue))
                    return ConverterResult.FromSuccess(enumValue);
                else
                    return ConverterResult.FromError(CommandError.ParseFailed, $"Value is not a {_enumType.Name}.");
            }
        }
    }
}
