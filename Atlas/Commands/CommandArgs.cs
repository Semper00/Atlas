using Atlas.Interfaces;

using Atlas.Pools;

using System.Linq;
using System.Collections.Generic;

namespace Atlas.Commands
{
    /// <summary>
    /// Used to simplify command arguments management.
    /// </summary>
#pragma warning disable CS0659
    public class CommandArgs : PoolableObject
    {
#pragma warning restore CS0659
        private IEnumerable<string> _args;

        public CommandArgs() { }

        /// <summary>
        /// Gets the argument count.
        /// </summary>
        public int Count => _args.Count();

        /// <summary>
        /// Gets the string array containing all arguments.
        /// </summary>
        public string[] Array => _args.ToArray();

        /// <summary>
        /// Gets all arguments in a string (split by case).
        /// </summary>
        public string String => ToString();

        /// <summary>
        /// Gets the string argument at a specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The string argument.</returns>
        public string this[int index] => _args.ElementAtOrDefault(index);

        /// <summary>
        /// Gets the string argument at a specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The string argument.</returns>
        public string Get(int index) => this[index];

        /// <summary>
        /// Safely gets the converted argument at a specified index.
        /// </summary>
        /// <typeparam name="T">The type to convert the argument to.</typeparam>
        /// <param name="index">The index.</param>
        /// <returns>The converted argument.</returns>
        public T Get<T>(int index)
        {
            IConverter converter = CommandManager.GetConverter<T>();

            if (converter != null)
                return (T) converter.Convert(Get(index));

            return default;
        }

        public override bool Equals(object obj)
        {
            if (obj is CommandArgs a)
            {
                if (a.Count != Count)
                    return false;

                for (int i = 0; i < Count; i++)
                {
                    if (a[i] != this[i])
                        return false;
                }
            }
            else if (obj is string[] array)
            {
                for (int i = 0; i < Count; i++)
                {
                    if (array.Length != Count)
                        return false;

                    if (array[i] != this[i])
                        return false;
                }
            }

            return base.Equals(obj);
        }

        public override string ToString()
            => string.Join(" ", _args);

        public override void OnReturned()
        {
            _args = null;
        }
    }
}