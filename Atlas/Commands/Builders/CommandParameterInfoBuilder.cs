using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using Atlas.Commands.Converters;
using Atlas.Commands.Entities;
using Atlas.Commands.Attributes;

namespace Atlas.Commands.Builders
{
    public class CommandParameterInfoBuilder
    {
        private readonly List<Attribute> _attributes;

        public CommandInfoBuilder Command { get; }

        public string Name { get; internal set; }

        public Type ParameterType { get; internal set; }
        public Converter Converter { get; set; }

        public bool IsOptional { get; set; }
        public bool IsRemainder { get; set; }
        public bool IsMultiple { get; set; }
        public object DefaultValue { get; set; }
        public string Summary { get; set; }

        public IReadOnlyList<Attribute> Attributes => _attributes;

        internal CommandParameterInfoBuilder(CommandInfoBuilder command)
        {
            _attributes = new List<Attribute>();

            Command = command;
        }

        internal CommandParameterInfoBuilder(CommandInfoBuilder command, string name, Type type)
            : this(command)
        {
            Name = name;
            SetType(type);
        }

        internal void SetType(Type type)
        {
            Converter = GetReader(type);

            if (type.GetTypeInfo().IsValueType)
                DefaultValue = Activator.CreateInstance(type);
            else if (type.IsArray)
                type = ParameterType.GetElementType();

            ParameterType = type;
        }

        private Converter GetReader(Type type)
        {
            var commands = Command.Module.Manager;

            if (type.GetTypeInfo().GetCustomAttribute<NamedArgumentTypeAttribute>() != null)
            {
                IsRemainder = true;

                var converter = commands.GetConverters(type)?.FirstOrDefault().Value;

                if (converter == null)
                {
                    Type readerType;

                    try
                    {
                        readerType = typeof(NamedArgumentConverter<>).MakeGenericType(new[] { type });
                    }
                    catch (ArgumentException ex)
                    {
                        throw new InvalidOperationException($"Parameter type '{type.Name}' for command '{Command.Name}' must be a class with a public parameterless constructor to use as a NamedArgumentType.", ex);
                    }

                    converter = (Converter)Activator.CreateInstance(readerType, new[] { commands });
                    commands.AddConverter(type, converter);
                }

                return converter;
            }


            var readers = commands.GetConverters(type);
            if (readers != null)
                return readers.FirstOrDefault().Value;
            else
                return commands.GetDefaultConverter(type);
        }

        public CommandParameterInfoBuilder WithSummary(string summary)
        {
            Summary = summary;
            return this;
        }

        public CommandParameterInfoBuilder WithDefault(object defaultValue)
        {
            DefaultValue = defaultValue;
            return this;
        }

        public CommandParameterInfoBuilder WithIsOptional(bool isOptional)
        {
            IsOptional = isOptional;
            return this;
        }

        public CommandParameterInfoBuilder WithIsRemainder(bool isRemainder)
        {
            IsRemainder = isRemainder;
            return this;
        }

        public CommandParameterInfoBuilder WithIsMultiple(bool isMultiple)
        {
            IsMultiple = isMultiple;
            return this;
        }

        public CommandParameterInfoBuilder AddAttributes(params Attribute[] attributes)
        {
            _attributes.AddRange(attributes);
            return this;
        }

        internal CommandParameterInfo Build(CommandInfo info)
        {
            if ((Converter ?? (Converter = GetReader(ParameterType))) == null)
                throw new InvalidOperationException($"No type reader found for type {ParameterType.Name}, one must be specified");

            return new CommandParameterInfo(this, info, Command.Module.Manager);
        }
    }
}