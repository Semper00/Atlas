using System;
using System.Linq;
using System.Collections.Generic;

using Atlas.Enums;

namespace Atlas.Commands.Attributes
{
    /// <summary>
    /// Used to set <see cref="CommandSource"/> environments for a command.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class CommandEnvironmentAttribute : Attribute
    {
        /// <summary>
        /// Gets the command environments.
        /// </summary>
        public HashSet<CommandSource> Environments { get; }

        public CommandEnvironmentAttribute(params CommandSource[] environments) => Environments = environments == null ? new HashSet<CommandSource>() : environments.ToHashSet();
    }
}