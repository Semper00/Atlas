using Atlas.Entities;
using Atlas.Commands.Enums;
using Atlas.Commands.Converters.Results;

namespace Atlas.Commands.Converters 
{
    public class DoorConverter : Converter
    {
        public override ConverterResult Convert(CommandContext context, string input)
        {
            Door door = Door.Get(input);

            if (door == null)
            {
                return ConverterResult.FromError(CommandError.ObjectNotFound, $"Failed to find that door.");
            }
            else
            {
                return ConverterResult.FromSuccess(door);
            }
        }
    }
}
