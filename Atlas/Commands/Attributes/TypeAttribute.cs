using System;
using System.Linq;
using System.Collections.Generic;

using Atlas.Commands.Enums;

namespace Atlas.Commands.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class TypeAttribute : Attribute
    {
        public HashSet<CommandType> Types { get; set; }

        public TypeAttribute(params CommandType[] type)
        {
            Types = type?.ToHashSet() ?? new HashSet<CommandType>() { CommandType.RemoteAdmin, CommandType.ServerConsole };
        }
    }
}
