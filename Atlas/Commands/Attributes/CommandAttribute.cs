using System;

namespace Atlas.Commands.Attributes
{
    /// <summary>
    /// Marks a method or a class as a commmand handler. When marking a class as a command handler, method names will be used as command names.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class CommandAttribute : Attribute
    {
        internal Type owner;
        internal bool nameSupplied;

        public CommandAttribute()
        {
            nameSupplied = false;
        }

        public CommandAttribute(string command, params string[] aliases)
        {
            nameSupplied = true;
            Command = command;
            Aliases = aliases;
        }

        public string Command { get; internal set; }
        public string[] Aliases { get; internal set; } 
    }
}