using System;

using Atlas.Entities;
using Atlas.Enums;
using Atlas.Commands.Enums;
using Atlas.Commands.Converters.Results;

namespace Atlas.Commands.Converters
{
    public class CameraConverter : Converter
    {
        public override ConverterResult Convert(CommandContext context, string input)
        {
            if (Enum.TryParse(input, out CameraType type))
            {
                Camera camera = Camera.Get(type);

                if (camera == null)
                {
                    return ConverterResult.FromError(CommandError.ObjectNotFound, $"Failed to find that camera.");
                }
                else
                {
                    return ConverterResult.FromSuccess(camera);
                }
            }

            return ConverterResult.FromError(CommandError.ParseFailed, $"Failed to parse {input} into a camera type.");
        }
    }
}