using System;
using System.Globalization;

using Atlas.Commands.Converters.Results;
using Atlas.Commands.Enums;

namespace Atlas.Commands.Converters
{
    internal class TimeSpanConverter : Converter
    {
        private static readonly string[] Formats = {
            "%d'd'%h'h'%m'm'%s's'", 
            "%d'd'%h'h'%m'm'",      
            "%d'd'%h'h'%s's'",     
            "%d'd'%h'h'",           
            "%d'd'%m'm'%s's'",
            "%d'd'%m'm'",           
            "%d'd'%s's'",       
            "%d'd'",            
            "%h'h'%m'm'%s's'",  
            "%h'h'%m'm'",       
            "%h'h'%s's'",         
            "%h'h'",        
            "%m'm'%s's'",     
            "%m'm'",               
            "%s's'",                
        };

        /// <inheritdoc />
        public override ConverterResult Convert(CommandContext context, string input)
        {
            return (TimeSpan.TryParseExact(input.ToLowerInvariant(), Formats, CultureInfo.InvariantCulture, out var timeSpan))
                ? ConverterResult.FromSuccess(timeSpan)
                : ConverterResult.FromError(CommandError.ParseFailed, "Failed to parse TimeSpan");
        }
    }
}
