using System;
using System.Linq;
using System.Collections.Generic;

namespace Atlas.Toolkit
{
    /// <summary>
    /// Provides helper methods for working with enums.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    public static class EnumHelper<T> where T : struct, Enum
    {
        public static List<T> All { get; } = Enum.GetValues(typeof(T)).Cast<T>().ToList();

        public static T AddTo(T value, T valueToAdd)
        {
            byte values = (byte)Enum.ToObject(typeof(T), value);
            byte toAdd = (byte)Enum.ToObject(typeof(T), valueToAdd);

            values += toAdd;

            return (T)Enum.ToObject(typeof(T), values);
        }

        public static T RemoveFrom(T value, T valueToRemove)
        {
            byte values = (byte)Enum.ToObject(typeof(T), value);
            byte toRemove = (byte)Enum.ToObject(typeof(T), valueToRemove);

            values -= toRemove;

            return (T)Enum.ToObject(typeof(T), values);
        }

        public static T AddTo(T value, IEnumerable<T> valuesToAdd)
        {
            foreach (T t in valuesToAdd)
            {
                value = AddTo(value, t);
            }

            return value;
        }

        public static T RemoveFrom(T value, IEnumerable<T> valuesToRemove)
        {
            foreach (T t in valuesToRemove)
            {
                value = RemoveFrom(value, t);
            }

            return value;
        }

        public static IEnumerable<T> GetValues(T value)
        {
            List<T> values = new List<T>();

            foreach (object obj in Enum.GetValues(typeof(T)))
            {
                if (value.HasFlag(obj as Enum))
                    values.Add((T)obj);
            }

            return values;
        }
    }
}
