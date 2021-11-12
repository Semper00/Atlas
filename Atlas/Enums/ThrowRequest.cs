namespace Atlas.Enums
{
    /// <summary>
    /// Possible throwable throw types.
    /// </summary>
    public enum ThrowRequest
    {
        /// <summary>
        /// Requesting to begin throwing a throwable item.
        /// </summary>
        BeginThrow,

        /// <summary>
        /// Requesting to confirm a weak throw.
        /// </summary>
        WeakThrow,

        /// <summary>
        /// Requesting to confirm a strong throw.
        /// </summary>
        FullForceThrow,
    }
}
