namespace Atlas.Enums
{
    /// <summary>
    /// An enum used to specify a command environment.
    /// </summary>
    public enum CommandSource
    {
        /// <summary>
        /// Specifies the Remote Admin Panel.
        /// </summary>
        RemoteAdmin,

        /// <summary>
        /// Specifies the player console (~).
        /// </summary>
        UserConsole,

        /// <summary>
        /// Specifies the server console (LocalAdmin/MultiAdmin).
        /// </summary>
        ServerConsole
    }
}