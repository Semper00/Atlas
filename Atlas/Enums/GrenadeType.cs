namespace Atlas.Enums
{
    /// <summary>
    /// The unique type of grenade.
    /// </summary>
    public enum GrenadeType
    {
        /// <summary>
        /// Frag grenade.
        /// Used by <see cref="ItemType.GrenadeFrag"/>.
        /// </summary>
        FragGrenade,

        /// <summary>
        /// Flashbang.
        /// Used by <see cref="ItemType.GrenadeFlash"/>.
        /// </summary>
        Flashbang,

        /// <summary>
        /// Scp018 ball.
        /// Used by <see cref="ItemType.SCP018"/>.
        /// </summary>
        Scp018,
    }
}
