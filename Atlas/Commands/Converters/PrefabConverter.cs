using System;

using Atlas.Entities;
using Atlas.Enums;
using Atlas.Commands.Enums;
using Atlas.Commands.Converters.Results;

namespace Atlas.Commands.Converters
{
    public class PrefabConverter : Converter
    {
        public override ConverterResult Convert(CommandContext context, string input)
        {
            if (Enum.TryParse(input, out PrefabType type))
            {
                Prefab prefab = Prefab.GetPrefab(type);

                if (prefab == null)
                {
                    return ConverterResult.FromError(CommandError.ObjectNotFound, $"Failed to find that prefab.");
                }
                else
                {
                    return ConverterResult.FromSuccess(prefab);
                }
            }

            return ConverterResult.FromError(CommandError.ParseFailed, $"Failed to parse {input} into a prefab type.");
        }
    }
}