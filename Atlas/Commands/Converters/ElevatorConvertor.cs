using System;

using Atlas.Entities;
using Atlas.Enums;
using Atlas.Commands.Enums;
using Atlas.Commands.Converters.Results;

namespace Atlas.Commands.Converters
{
    public class ElevatorConvertor : Converter
    {
        public override ConverterResult Convert(CommandContext context, string input)
        {
            if (Enum.TryParse(input, out ElevatorType type))
            {
                Elevator elevator = Elevator.Get(type);

                if (elevator == null)
                {
                    return ConverterResult.FromError(CommandError.ObjectNotFound, $"Failed to find that elevator.");
                }
                else
                {
                    return ConverterResult.FromSuccess(elevator);
                }
            }

            return ConverterResult.FromError(CommandError.ParseFailed, $"Failed to parse {input} into an elevator type.");
        }
    }
}