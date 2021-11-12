using UnityEngine;

namespace Atlas.Extensions
{
    public static class UnityExtensions
    {
        /// <summary>
        /// Gets the component in parent.
        /// </summary>
        /// <typeparam name="T">The component type.</typeparam>
        /// <param name="obj">The object to get the component from.</param>
        /// <param name="component">The component found, if any.</param>
        /// <returns>true if the component was found, otherwise false.</returns>
        public static bool TryGetComponentInParent<T>(this Component obj, out T component) where T : Component
        {
            component = obj.GetComponentInParent<T>();

            return component != null;
        }
    }
}
