using System;

namespace Atlas.Commands.Attributes
{
    /// <summary>
    /// Used to set command usage for a command.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class CommandUsageAttribute : Attribute
    {
        public string Usage { get; }

        public CommandUsageAttribute(string usage) => Usage = usage;
    }
}
