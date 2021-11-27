using Atlas.Commands.Converters.Results;

namespace Atlas.Commands.Converters
{
    public abstract class Converter
    {
        public abstract ConverterResult Convert(CommandContext context, string input);
    }
}
