using System;
using System.Reflection;
using System.Collections.Generic;

using Atlas.Commands.Entities;
using Atlas.Commands.Interfaces;

namespace Atlas.Commands.Builders
{
    public class CommandModuleInfoBuilder
    {
        private readonly List<CommandInfoBuilder> _commands;
        private readonly List<CommandModuleInfoBuilder> _submodules;
        private readonly List<Attribute> _attributes;
        private readonly List<string> _aliases;

        public CommandManager Manager { get; }
        public CommandModuleInfoBuilder Parent { get; }

        public string Name { get; set; }
        public string Summary { get; set; }

        public IReadOnlyList<CommandInfoBuilder> Commands => _commands;
        public IReadOnlyList<CommandModuleInfoBuilder> Modules => _submodules;
        public IReadOnlyList<Attribute> Attributes => _attributes;
        public IReadOnlyList<string> Aliases => _aliases;

        internal TypeInfo TypeInfo { get; set; }

        internal CommandModuleInfoBuilder(CommandManager manager, CommandModuleInfoBuilder parent)
        {
            Manager = manager;
            Parent = parent;

            _commands = new List<CommandInfoBuilder>();
            _submodules = new List<CommandModuleInfoBuilder>();
            _attributes = new List<Attribute>();
            _aliases = new List<string>();
        }

        internal CommandModuleInfoBuilder(CommandManager manager, CommandModuleInfoBuilder parent, string primaryAlias)
            : this(manager, parent)
        {
            _aliases = new List<string> { primaryAlias };
        }

        public CommandModuleInfoBuilder WithName(string name)
        {
            Name = name;
            return this;
        }

        public CommandModuleInfoBuilder WithSummary(string summary)
        {
            Summary = summary;
            return this;
        }

        public CommandModuleInfoBuilder AddAliases(params string[] aliases)
        {
            for (int i = 0; i < aliases.Length; i++)
            {
                string alias = aliases[i] ?? "";

                if (!_aliases.Contains(alias))
                    _aliases.Add(alias);
            }
            return this;
        }

        public CommandModuleInfoBuilder AddAttributes(params Attribute[] attributes)
        {
            _attributes.AddRange(attributes);
            return this;
        }

        public CommandModuleInfoBuilder AddCommand(string primaryAlias, Func<CommandContext, object[], CommandInfo, IResult> callback, Action<CommandInfoBuilder> createFunc)
        {
            var builder = new CommandInfoBuilder(this, primaryAlias, callback);
            createFunc(builder);
            _commands.Add(builder);
            return this;
        }

        internal CommandModuleInfoBuilder AddCommand(Action<CommandInfoBuilder> createFunc)
        {
            var builder = new CommandInfoBuilder(this);
            createFunc(builder);
            _commands.Add(builder);
            return this;
        }

        public CommandModuleInfoBuilder AddModule(string primaryAlias, Action<CommandModuleInfoBuilder> createFunc)
        {
            var builder = new CommandModuleInfoBuilder(Manager, this, primaryAlias);
            createFunc(builder);
            _submodules.Add(builder);
            return this;
        }

        internal CommandModuleInfoBuilder AddModule(Action<CommandModuleInfoBuilder> createFunc)
        {
            var builder = new CommandModuleInfoBuilder(Manager, this);
            createFunc(builder);
            _submodules.Add(builder);
            return this;
        }

        private CommandModuleInfo BuildImpl(CommandManager manager, CommandModuleInfo parent = null)
        {
            if (Name == null)
                Name = _aliases[0];

            return new CommandModuleInfo(this, manager, parent);
        }

        public CommandModuleInfo Build(CommandManager manager) => BuildImpl(manager);

        internal CommandModuleInfo Build(CommandManager manager, CommandModuleInfo parent) => BuildImpl(manager, parent);
    }
}
