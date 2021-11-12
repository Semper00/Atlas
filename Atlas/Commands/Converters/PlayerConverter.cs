using Atlas.Interfaces;
using Atlas.Entities;

namespace Atlas.Commands.Converters
{
    /// <summary>
    /// A default <see cref="Player"/> converter.
    /// </summary>
    public class PlayerConverter : IConverter
    {
        public object Convert(string value)
            => PlayersList.Get(value);
    }
}