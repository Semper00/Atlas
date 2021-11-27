using System;

namespace Atlas.Commands.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class NamedArgumentTypeAttribute : Attribute { }
}
