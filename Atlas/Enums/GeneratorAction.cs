namespace Atlas.Enums
{
    /// <summary>
    /// An enum representing in-game generator operations.
    /// </summary>
    public enum GeneratorAction
    {
        /// <summary>
        /// Someone is unlocking the generator.
        /// </summary>
        Unlocking,

        /// <summary>
        /// Someone is closing the generator's doors.
        /// </summary>
        Closing,

        /// <summary>
        /// Someone is opening the generator's doors.
        /// </summary>
        Opening,

        /// <summary>
        /// Someone is starting the generator.
        /// </summary>
        Starting,

        /// <summary>
        /// The generator is stopping.
        /// </summary>
        Stopping
    }
}
