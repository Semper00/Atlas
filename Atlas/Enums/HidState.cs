namespace Atlas.Enums
{
    /// <summary>
    /// Possible <see cref="Entities.Items.MicroHidItem"/> states.
    /// </summary>
    public enum HidState
    {
        /// <summary>
        /// Idling and not using energy.
        /// </summary>
        Idle,

        /// <summary>
        /// Powering up and using energy slowly.
        /// </summary>
        PoweringUp,

        /// <summary>
        /// Powering down and not using energy.
        /// </summary>
        PoweringDown,

        /// <summary>
        /// Fully powered up and ready to fire.
        /// </summary>
        Primed,

        /// <summary>
        /// Firing and using energy.
        /// </summary>
        Firing,
    }
}
