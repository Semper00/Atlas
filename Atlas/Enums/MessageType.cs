namespace Atlas.Enums
{
    /// <summary>
    /// Determines the type of a message.
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// Indicates that this is a broadcast.
        /// </summary>
        Broadcast,

        /// <summary>
        /// Indicates that this is a monospaced broadcast.
        /// </summary>
        BroadcastMono,

        /// <summary>
        /// Indicates that this is a admin chat message.
        /// </summary>
        AdminChat,

        /// <summary>
        /// Indicates that this is a hint.
        /// </summary>
        Hint,

        /// <summary>
        /// Indicates that this is a player report.
        /// </summary>
        PlayerReport,

        /// <summary>
        /// Indicates that this is a console message.
        /// </summary>
        Console,

        /// <summary>
        /// Indicates that this is a remote admin panel message.
        /// </summary>
        RemoteAdmin
    }
}
