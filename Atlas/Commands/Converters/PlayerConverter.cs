using Atlas.Entities;
using Atlas.Commands.Enums;
using Atlas.Commands.Converters.Results;

namespace Atlas.Commands.Converters
{
    public class PlayerConverter : Converter
    {
        public override ConverterResult Convert(CommandContext context, string input)
        {
            if (!PlayersList.TryGet(input, out Player player))
            {
                return ConverterResult.FromError(CommandError.ObjectNotFound, $"Failed to find that player.");
            }
            else
            {
                return ConverterResult.FromSuccess(player);
            }
        }
    }
}
