using System;

namespace Atlas.Commands.Attributes
{
    /// <summary>
    /// Used to set a description for a command.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class CommandDescriptionAttribute : Attribute
    {
        public string Description { get; }

        public CommandDescriptionAttribute(string description) => Description = description;
    }
}