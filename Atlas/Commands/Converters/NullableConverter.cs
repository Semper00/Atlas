using System;
using System.Linq;
using System.Reflection;

using Atlas.Commands.Converters.Results;

namespace Atlas.Commands.Converters
{
    internal static class NullableConverter
    {
        public static Converter Create(Type type, Converter converter)
        {
            var constructor = typeof(NullableConverter<>).MakeGenericType(type).GetTypeInfo().DeclaredConstructors.First();

            return (Converter)constructor.Invoke(new object[] { converter });
        }
    }

    internal class NullableConverter<T> : Converter
        where T : struct
    {
        private readonly Converter _baseConverter;

        public NullableConverter(Converter baseConverter)
        {
            _baseConverter = baseConverter;
        }

        public override ConverterResult Convert(CommandContext context, string input)
        {
            if (string.Equals(input, "null", StringComparison.OrdinalIgnoreCase) || string.Equals(input, "nothing", StringComparison.OrdinalIgnoreCase))
                return ConverterResult.FromSuccess(new T?());

            return _baseConverter.Convert(context, input);
        }
    }
}
