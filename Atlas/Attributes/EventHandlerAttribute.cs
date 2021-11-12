using System;

namespace Atlas.Attributes
{
    /// <summary>
    /// An attribute used to indicate a method as an event handler.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class EventHandlerAttribute : Attribute
    {
        /// <summary>
        /// The event type.
        /// </summary>
        public Type EventType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventHandlerAttribute"/> attribute.
        /// </summary>
        /// <param name="type">The type of the target event, for example: typeof(PlayerDiedEvent)</param>
        public EventHandlerAttribute(Type type)
        {
            if (!type.IsSubclassOf(typeof(EventSystem.Event)))
                throw new InvalidOperationException($"{type.FullName} is not an event!");

            EventType = type;
        }
    }
}