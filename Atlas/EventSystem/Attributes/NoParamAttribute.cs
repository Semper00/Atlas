using System;

namespace Atlas.EventSystem.Attributes
{
    /// <summary>
    /// Used to tell the <see cref="EventManager"/> that your method does not require an event parameter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class NoParamAttribute : Attribute
    {
    }
}
