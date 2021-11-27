using System;
using System.Collections.Generic;
using System.Linq;

using Atlas.Commands.Entities;
using Atlas.Commands.Interfaces;
using Atlas.Commands.Enums;

namespace Atlas.Commands.Builders
{
    public class CommandInfoBuilder
    {
        private readonly List<CommandParameterInfoBuilder> _parameters;
        private readonly List<Attribute> _attributes;
        private readonly List<string> _aliases;

        public CommandModuleInfoBuilder Module { get; }

        internal Func<CommandContext, object[], CommandInfo, IResult> Callback { get; set; }

        public string Name { get; set; }
        public string Summary { get; set; }
        public string Remarks { get; set; }
        public string PrimaryAlias { get; set; }
        public int Priority { get; set; }
        public bool IgnoreExtraArgs { get; set; }
        public HashSet<CommandType> Types { get; set; }

        public IReadOnlyList<CommandParameterInfoBuilder> Parameters => _parameters;
        public IReadOnlyList<Attribute> Attributes => _attributes;
        public IReadOnlyList<string> Aliases => _aliases;

        internal CommandInfoBuilder(CommandModuleInfoBuilder module)
        {
            Module = module;

            _parameters = new List<CommandParameterInfoBuilder>();
            _attributes = new List<Attribute>();
            _aliases = new List<string>();
        }

        internal CommandInfoBuilder(CommandModuleInfoBuilder module, string primaryAlias, Func<CommandContext, object[], CommandInfo, IResult> callback)
            : this(module)
        {
            Callback = callback;
            PrimaryAlias = primaryAlias;
            _aliases.Add(primaryAlias);
        }

        public CommandInfoBuilder WithName(string name)
        {
            Name = name;
            return this;
        }

        public CommandInfoBuilder WithSummary(string summary)
        {
            Summary = summary;
            return this;
        }

        public CommandInfoBuilder WithRemarks(string remarks)
        {
            Remarks = remarks;
            return this;
        }

        public CommandInfoBuilder WithPriority(int priority)
        {
            Priority = priority;
            return this;
        }

        public CommandInfoBuilder WithType(IEnumerable<CommandType> type)
        {
            if (Types == null)
                Types = new HashSet<CommandType>(type);
            else
                Types.UnionWith(type);
            
            return this;
        }

        public CommandInfoBuilder AddAliases(params string[] aliases)
        {
            for (int i = 0; i < aliases.Length; i++)
            {
                string alias = aliases[i] ?? "";
                if (!_aliases.Contains(alias))
                    _aliases.Add(alias);
            }
            return this;
        }

        public CommandInfoBuilder AddAttributes(params Attribute[] attributes)
        {
            _attributes.AddRange(attributes);
            return this;
        }

        public CommandInfoBuilder AddParameter<T>(string name, Action<CommandParameterInfoBuilder> createFunc)
        {
            var param = new CommandParameterInfoBuilder(this, name, typeof(T));
            createFunc(param);
            _parameters.Add(param);
            return this;
        }

        public CommandInfoBuilder AddParameter(string name, Type type, Action<CommandParameterInfoBuilder> createFunc)
        {
            var param = new CommandParameterInfoBuilder(this, name, type);
            createFunc(param);
            _parameters.Add(param);
            return this;
        }

        internal CommandInfoBuilder AddParameter(Action<CommandParameterInfoBuilder> createFunc)
        {
            var param = new CommandParameterInfoBuilder(this);
            createFunc(param);
            _parameters.Add(param);
            return this;
        }

        internal CommandInfo Build(CommandModuleInfo info, CommandManager manager)
        {
            if (Name == null)
                Name = PrimaryAlias;

            if (_parameters.Count > 0)
            {
                var lastParam = _parameters[_parameters.Count - 1];

                var firstMultipleParam = _parameters.FirstOrDefault(x => x.IsMultiple);

                if ((firstMultipleParam != null) && (firstMultipleParam != lastParam))
                    throw new InvalidOperationException($"Only the last parameter in a command may have the Multiple flag. Parameter: {firstMultipleParam.Name} in {PrimaryAlias}");

                var firstRemainderParam = _parameters.FirstOrDefault(x => x.IsRemainder);

                if ((firstRemainderParam != null) && (firstRemainderParam != lastParam))
                    throw new InvalidOperationException($"Only the last parameter in a command may have the Remainder flag. Parameter: {firstRemainderParam.Name} in {PrimaryAlias}");
            }

            return new CommandInfo(this, info, manager);
        }
    }
}
