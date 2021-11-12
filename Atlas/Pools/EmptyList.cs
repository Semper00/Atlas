using System.Collections.Generic;

namespace Atlas.Pools
{
    /// <summary>
    /// Gives you an empty list.
    /// </summary>
    /// <typeparam name="T">The generic type to get a list of.</typeparam>
    public static class EmptyList<T>
    {
        /// <summary>
        /// The empty list.
        /// </summary>
        public static IReadOnlyList<T> List = new List<T>(1);
    }
}
