using System.Collections.Generic;
using System.Linq;

namespace Atlas.Toolkit
{
    /// <summary>
    /// A little tool to help with session management.
    /// </summary>
    public class Container
    {
        /// <summary>
        /// Initialzes a new container.
        /// </summary>
        public Container() 
            => ContentHolder = new Dictionary<string, object>();

        private Dictionary<string, object> ContentHolder { get; }

        /// <summary>
        /// Inserts a new value.
        /// </summary>
        /// <param name="key">Key to this value.</param>
        /// <param name="value">The value.</param>
        public void InsertValue(string key, object value)
            => ContentHolder.Add(key, value);

        /// <summary>
        /// Sets a value of a key. This will insert a new value of this key doesn't exist.
        /// </summary>
        /// <param name="key">The key to this value.</param>
        /// <param name="value">The value to set.</param>
        public void SetValue(string key, object value)
        {
            if (!ContentHolder.ContainsKey(key))
                InsertValue(key, value);
            else
                ContentHolder[key] = value;
        }

        /// <summary>
        /// Nulls a value.
        /// </summary>
        /// <param name="key">The key to this value.</param>
        public void NullValue(string key) => SetValue(key, null);

        /// <summary>
        /// Removes a value.
        /// </summary>
        /// <param name="key">The key to this value.</param>
        /// <returns><see cref="true"/> if the value was removed succesfully, otherwise <see cref="false"/>.</returns>
        public bool RemoveValue(string key) => ContentHolder.Remove(key);

        /// <summary>
        /// Gets a value.
        /// </summary>
        /// <param name="key">The key to this value.</param>
        /// <returns>The value as an <see cref="object"/>.</returns>
        public object GetValue(string key) => ContentHolder.TryGetValue(key, out object value) ? value : null;

        /// <summary>
        /// Gets a value.
        /// </summary>
        /// <typeparam name="T">The type of the returned value.</typeparam>
        /// <param name="key">The key to this value.</param>
        /// <returns>The value as the specified type if they match, otherwise <see cref="default"/>.</returns>
        public T GetValue<T>(string key)
        {
            object value = GetValue(key);

            if (value != null && value is T t)
                return t;

            return default;
        }

        /// <summary>
        /// Gets a <see cref="HashSet{T}"/> of all keys and values.
        /// </summary>
        /// <returns>The <see cref="HashSet{T}"/> of all keys and values.</returns>
        public HashSet<KeyValuePair<string, object>> GetContainer() 
            => ContentHolder.ToHashSet();

        /// <summary>
        /// Gets a <see cref="HashSet{T}"/> of all keys.
        /// </summary>
        /// <returns>The <see cref="HashSet{T}"/> containing all registered keys.</returns>
        public HashSet<string> GetKeys()
            => ContentHolder.Keys.ToHashSet();

        /// <summary>
        /// Gets a <see cref="HashSet{T}"/> of all values.
        /// </summary>
        /// <returns>The <see cref="HashSet{T}"/> containing all registered values.</returns>
        public HashSet<object> GetValues()
            => ContentHolder.Values.ToHashSet();
    }
}