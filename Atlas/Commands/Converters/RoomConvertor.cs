using System;
using System.Linq;

using Atlas.Entities;
using Atlas.Enums;
using Atlas.Commands.Enums;
using Atlas.Commands.Converters.Results;

namespace Atlas.Commands.Converters
{
    public class RoomConvertor : Converter
    {
        public override ConverterResult Convert(CommandContext context, string input)
        {
            if (Enum.TryParse(input, out RoomType type))
            {
                Room room = Map.Rooms.FirstOrDefault(x => x.Type == type);

                if (room == null)
                {
                    return ConverterResult.FromError(CommandError.ObjectNotFound, $"Failed to find that room.");
                }
                else
                {
                    return ConverterResult.FromSuccess(room);
                }
            }
            else
            {
                Room room = Map.Rooms.FirstOrDefault(x => x.Name != null && x.Name == input);

                if (room == null)
                {
                    return ConverterResult.FromError(CommandError.ObjectNotFound, $"Failed to find that room.");
                }
                else
                {
                    return ConverterResult.FromSuccess(room);
                }
            }

            return ConverterResult.FromError(CommandError.ParseFailed, $"Failed to parse {input} into a room type.");
        }
    }
}