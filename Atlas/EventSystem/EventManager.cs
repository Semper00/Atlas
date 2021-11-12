using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using Atlas.Attributes;
using Atlas.EventSystem.Attributes;
using Atlas.EventSystem.Enums;

namespace Atlas.EventSystem
{
    /// <summary>
    /// A class for event system management.
    /// </summary>
    public static class EventManager
    {
        internal static List<EventHandler> handlers;

        static EventManager()
        {
            handlers = new List<EventHandler>();

            Register(typeof(EventManager).Assembly);
        }

        /// <summary>
        /// Gets a list of all registered event handlers.
        /// </summary>
        public static IReadOnlyList<EventHandler> EventHandlers => handlers;

        /// <summary>
        /// Invokes an event by it's type.
        /// </summary>
        /// <param name="ev">The event to invoke.</param>
        public static TEvent Invoke<TEvent>(TEvent instance = null) where TEvent : Event
        {
            Type evType = typeof(TEvent);

            foreach (EventHandler handler in handlers)
            {
                if (handler.Event == evType)
                {
                    try
                    {
                        handler.Invoke(instance);
                    }
                    catch (Exception e)
                    {
                        Log.Error("Atlas", $"An error occured while executing an Event Handler ({handler})!");
                        Log.Error("Atlas", e);

                        continue;
                    }
                }
            }

            return instance;
        }

        /// <summary>
        /// Gets a <see cref="HashSet{T}"/> of all event handlers located in an assembly.
        /// </summary>
        /// <param name="assembly">The assembly to search in.</param>
        /// <returns>The <see cref="HashSet{T}"/> of all event handlers.</returns>
        public static HashSet<EventHandler> GetHandlers(Assembly assembly)
            => handlers.Where(x => assembly.GetTypes().Contains(x.Owner)).ToHashSet();

        /// <summary>
        /// Registers all methods marked as event handlers.
        /// </summary>
        /// <typeparam name="T">The class to register from.</typeparam>
        public static void Register<T>(T instance)
            => Register(typeof(T), instance);

        /// <summary>
        /// Registers all methods marked as event handlers from an assembly.
        /// </summary>
        /// <param name="assembly">The assembly to search in.</param>
        public static void Register(Assembly assembly)
            => assembly.GetTypes().Where(x => x.IsPublic).ToList().ForEach(x => Register(x));

        /// <summary>
        /// Registers all methods marked as event handlers.
        /// </summary>
        /// <param name="type">The type to register from.</param>
        public static void Register(Type type, object instance)
        {
            // this happened for some reason
            if (type == typeof(Type))
                return;

            foreach (MethodInfo method in type.GetMethods())
            {
                EventHandlerAttribute attribute = method.GetCustomAttribute<EventHandlerAttribute>();

                if (attribute != null)
                {
                    if (attribute.EventType.GetCustomAttribute<NoParamAttribute>() != null)
                    {
                        try
                        {
                            EventHandler handler = new EventHandler(type, attribute.EventType, method, DelegateType.Nullable, instance);

                            handlers.Add(handler);
                        }
                        catch (Exception e)
                        {
                            Log.Error("Atlas", e);

                            continue;
                        }
                    }
                    else
                    {

                        ParameterInfo[] parameters = method.GetParameters();

                        if (parameters == null || parameters.Length != 1)
                            throw new InvalidOperationException($"A method marked as an event handler MUST contain only one parameter, no more and no less.\nSource: {type.FullName}.{method.Name}");

                        if (parameters[0].ParameterType != attribute.EventType || (!parameters[0].ParameterType.IsSubclassOf(typeof(BoolEvent)) && !parameters[0].ParameterType.BaseType.IsSubclassOf(typeof(Event))))
                            throw new InvalidOperationException($"The event type in the {typeof(EventHandlerAttribute).FullName} attribute MUST match the type of the first parameter.\nSource: {type.FullName}.{method.Name}");

                        try
                        {
                            EventHandler handler = new EventHandler(type, attribute.EventType, method, DelegateType.Target, instance);

                            handlers.Add(handler);
                        }
                        catch (Exception e)
                        {
                            Log.Error("Atlas", e);

                            continue;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Unregisters all event handlers in this type.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        public static void Unregister<T>()
            => Unregister(typeof(T));

        /// <summary>
        /// Unregisters all event handlers in this assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public static void Unregister(Assembly assembly)
            => assembly.GetTypes().Where(x => x.IsPublic).ToList().ForEach(x => Unregister(x));

        /// <summary>
        /// Unregisters all event handlers in this type.
        /// </summary>
        /// <param name="type">The type.</param>
        public static void Unregister(Type type)
        {
            var handlers = EventManager.handlers;

            for (int i = 0; i < handlers.Count; i++)
            {
                EventHandler handler = handlers[i];

                if (handler.Owner == type)
                    EventManager.handlers.RemoveAt(i);
            }
        }

        /// <summary>
        /// Sorts Event Handlers by their priority.
        /// </summary>
        public static void SortHandlers()
        {
            var sorted = handlers.OrderBy(x => x.Priority);

            handlers.Clear();
            handlers.AddRange(sorted);
        }
    }

    /// <summary>
    /// An abstract class for event management.
    /// </summary>
    public abstract class BoolEvent : Event
    {
        /// <summary>
        /// Gets or sets a value indicating whether or not to allow this event.
        /// <b>Warning: this may not work on some events.</b>
        /// </summary>
        public bool IsAllowed { get; set; }
    }

    /// <summary>
    /// An abstract class for event management (without a boolean).
    /// </summary>
    public abstract class Event 
    {

    }

    /// <summary>
    /// A class for event handler wrapping.
    /// </summary>
    public class EventHandler
    {
        /// <summary>
        /// Gets the owner of this wrapper.
        /// </summary>
        public Type Owner { get; }

        /// <summary>
        /// Gets the event type.
        /// </summary>
        public Type Event { get; }

        /// <summary>
        /// Gets the delegate of the event.
        /// </summary>
        public MethodInfo Method { get; }

        /// <summary>
        /// Gets the object the method is going to be invoked on.
        /// </summary>
        public object Invoker { get; }

        /// <summary>
        /// Gets the handler's priority.
        /// </summary>
        public int Priority { get; }

        /// <summary>
        /// Gets the delegate's type.
        /// </summary>
        public DelegateType Type { get; }

        /// <summary>
        /// Initializes a new <see cref="EventHandler"/> wrapper.
        /// </summary>
        /// <param name="owner">The owner of this wrapper.</param>
        /// <param name="ev">The event type.</param>
        /// <param name="deleg">The delegate method.</param>
        public EventHandler(Type owner, Type ev, MethodInfo method, DelegateType type, object invoker)
        {
            Owner = owner;
            Event = ev;
            Type = type;
            Method = method;
            Invoker = invoker;
            Priority = owner.Assembly == typeof(EventManager).Assembly ? 1 : 0;

            Log.Dev($"Event Handler created: {owner.FullName}.{method.Name}@{ev.FullName}@{type}");

            EventManager.SortHandlers();
        }

        public void Invoke(Event ev = null)
        {
            if (Type == DelegateType.Nullable)
                Method.Invoke(Invoker, null);
            else
                Method.Invoke(Invoker, new object[] { ev });
        }

        public override string ToString()
            => Owner.FullName + "." + Method.Name + " | " + Event.FullName;
    }
}