namespace Atlas.Enums
{
    /// <summary>
    /// An enum indicating the current intercom state.
    /// </summary>
    public enum IntercomState
    {
        /// <summary>
        /// The intercom is ready to transmit.
        /// </summary>
        Ready,

        /// <summary>
        /// The intercom is transmitting.
        /// </summary>
        Transmitting,

        /// <summary>
        /// The intercom is transmitting with bypass mode.
        /// </summary>
        TransmittingBypass,

        /// <summary>
        /// The intercom is restarting.
        /// </summary>
        Restarting,

        /// <summary>
        /// The intercom is being used by someone with admin speaking mode.
        /// </summary>
        AdminSpeaking,
        
        /// <summary>
        /// The intercom is being used by someone muted.
        /// </summary>
        Muted,

        /// <summary>
        /// The intercom has custom content enabled.
        /// </summary>
        Custom
    }
}
