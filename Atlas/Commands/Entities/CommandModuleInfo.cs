using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Atlas.Commands.Builders;
using Atlas.Commands.Extensions;

namespace Atlas.Commands.Entities
{
    public class CommandModuleInfo
    {
        public CommandManager Manager { get; }

        public string Name { get; }

        public string Summary { get; }

        public IReadOnlyList<string> Aliases { get; }

        public IReadOnlyList<CommandInfo> Commands { get; }

        public IReadOnlyList<Attribute> Attributes { get; }

        public IReadOnlyList<CommandModuleInfo> Submodules { get; }

        public CommandModuleInfo Parent { get; }

        public bool IsSubmodule => Parent != null;

        internal CommandModuleInfo(CommandModuleInfoBuilder builder, CommandManager manager, CommandModuleInfo parent = null)
        {
            Manager = manager;

            Name = builder.Name;
            Summary = builder.Summary;
            Parent = parent;

            Aliases = BuildAliases(builder, manager).ToImmutableArray();
            Commands = builder.Commands.Select(x => x.Build(this, manager)).ToImmutableArray();
            Attributes = BuildAttributes(builder).ToImmutableArray();

            Submodules = BuildSubmodules(builder, manager).ToImmutableArray();
        }

        private static IEnumerable<string> BuildAliases(CommandModuleInfoBuilder builder, CommandManager manager)
        {
            var result = builder.Aliases.ToList();
            var builderQueue = new Queue<CommandModuleInfoBuilder>();

            var parent = builder;

            while ((parent = parent.Parent) != null)
                builderQueue.Enqueue(parent);

            while (builderQueue.Count > 0)
            {
                var level = builderQueue.Dequeue();

                result = level.Aliases.Permutate(result, (first, second) =>
                {
                    if (first == "")
                        return second;
                    else if (second == "")
                        return first;
                    else
                        return first + manager.Config.SeparatorChar + second;
                }).ToList();
            }

            return result;
        }

        private List<CommandModuleInfo> BuildSubmodules(CommandModuleInfoBuilder parent, CommandManager manager)
        {
            var result = new List<CommandModuleInfo>();

            foreach (var submodule in parent.Modules)
                result.Add(submodule.Build(manager, this));

            return result;
        }

        private static List<Attribute> BuildAttributes(CommandModuleInfoBuilder builder)
        {
            var result = new List<Attribute>();

            CommandModuleInfoBuilder parent = builder;

            while (parent != null)
            {
                result.AddRange(parent.Attributes);
                parent = parent.Parent;
            }

            return result;
        }
    }
}
