using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Atlas.Commands.Converters
{
    internal delegate bool TryParseDelegate<T>(string str, out T value);

    internal static class PrimitiveConverters
    {
        internal static readonly Lazy<IReadOnlyDictionary<Type, Delegate>> Converters = new Lazy<IReadOnlyDictionary<Type, Delegate>>(Create);

        public static IEnumerable<Type> SupportedTypes = Converters.Value.Keys;

        internal static IReadOnlyDictionary<Type, Delegate> Create()
        {
            var parserBuilder = ImmutableDictionary.CreateBuilder<Type, Delegate>();

            parserBuilder[typeof(bool)] = (TryParseDelegate<bool>)bool.TryParse;
            parserBuilder[typeof(sbyte)] = (TryParseDelegate<sbyte>)sbyte.TryParse;
            parserBuilder[typeof(byte)] = (TryParseDelegate<byte>)byte.TryParse;
            parserBuilder[typeof(short)] = (TryParseDelegate<short>)short.TryParse;
            parserBuilder[typeof(ushort)] = (TryParseDelegate<ushort>)ushort.TryParse;
            parserBuilder[typeof(int)] = (TryParseDelegate<int>)int.TryParse;
            parserBuilder[typeof(uint)] = (TryParseDelegate<uint>)uint.TryParse;
            parserBuilder[typeof(long)] = (TryParseDelegate<long>)long.TryParse;
            parserBuilder[typeof(ulong)] = (TryParseDelegate<ulong>)ulong.TryParse;
            parserBuilder[typeof(float)] = (TryParseDelegate<float>)float.TryParse;
            parserBuilder[typeof(double)] = (TryParseDelegate<double>)double.TryParse;
            parserBuilder[typeof(decimal)] = (TryParseDelegate<decimal>)decimal.TryParse;
            parserBuilder[typeof(DateTime)] = (TryParseDelegate<DateTime>)DateTime.TryParse;
            parserBuilder[typeof(DateTimeOffset)] = (TryParseDelegate<DateTimeOffset>)DateTimeOffset.TryParse;
            parserBuilder[typeof(char)] = (TryParseDelegate<char>)char.TryParse;

            return parserBuilder.ToImmutable();
        }

        public static TryParseDelegate<T> Get<T>() => (TryParseDelegate<T>)Converters.Value[typeof(T)];
        public static Delegate Get(Type type) => Converters.Value[type];
    }
}
