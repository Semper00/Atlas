namespace Atlas.Enums
{
    /// <summary>
    /// An enum for generator flags.
    /// </summary>
    public enum GeneratorFlags : byte
    {
        /// <summary>
        /// No flags.
        /// </summary>
        None = 1,
        
        /// <summary>
        /// Doors unlocked.
        /// </summary>
        Unlocked = 2,

        /// <summary>
        /// Doors opened.
        /// </summary>
        Open = 4,

        /// <summary>
        /// Generator activated.
        /// </summary>
        Activating = 8,

        /// <summary>
        /// Generator engaged.
        /// </summary>
        Engaged = 16
    }
}
