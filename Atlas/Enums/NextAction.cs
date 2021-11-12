namespace Atlas.Enums
{
    /// <summary>
    /// An enum for actions when the round ends.
    /// </summary>
    public enum NextAction : byte
    {
        /// <summary>
        /// Nothing.
        /// </summary>
        Nothing = 0x00,

        /// <summary>
        /// Restart the server.
        /// </summary>
        Restart = 0x01,

        /// <summary>
        /// Shut the server down.
        /// </summary>
        Shutdown = 0x02
    }
}
