namespace Atlas.Interfaces
{
    /// <summary>
    /// An interface used to indicate that a class is an argument converter.
    /// </summary>
    public interface IConverter
    {
        /// <summary>
        /// Converts the string value.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        object Convert(string value);
    }
}
