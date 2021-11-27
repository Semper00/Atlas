using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using Atlas.Commands.Builders;
using Atlas.Commands.Converters;
using Atlas.Commands.Converters.Results;

namespace Atlas.Commands.Entities
{
    public class CommandParameterInfo
    {
        private readonly Converter _converter;

        public CommandInfo Command { get; }

        public string Name { get; }

        public string Summary { get; }

        public bool IsOptional { get; }

        public bool IsRemainder { get; }
        public bool IsMultiple { get; }

        public Type Type { get; }

        public object DefaultValue { get; }

        public IReadOnlyList<Attribute> Attributes { get; }

        internal CommandParameterInfo(CommandParameterInfoBuilder builder, CommandInfo command, CommandManager manager)
        {
            Command = command;

            Name = builder.Name;
            Summary = builder.Summary;
            IsOptional = builder.IsOptional;
            IsRemainder = builder.IsRemainder;
            IsMultiple = builder.IsMultiple;

            Type = builder.ParameterType;
            DefaultValue = builder.DefaultValue;

            Attributes = builder.Attributes.ToImmutableArray();

            _converter = builder.Converter;
        }

        public ConverterResult Parse(CommandContext context, string input)
        {
            return _converter.Convert(context, input);
        }

        public override string ToString() => Name;
    }
}
