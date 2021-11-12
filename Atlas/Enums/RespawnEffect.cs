namespace Atlas.Enums
{
    /// <summary>
    /// Layers game respawn effects.
    /// </summary>
    public enum RespawnEffect : byte
    {
        /// <summary>
        /// Plays the <see cref="RoleType.ChaosInsurgency"/> music to alive <see cref="RoleType.ClassD"/> and <see cref="RoleType.ChaosInsurgency"/>.
        /// </summary>
        PlayChaosInsurgencyMusic = 0,

        /// <summary>
        /// Summons the <see cref="RoleType.ChaosInsurgency"/> van.
        /// </summary>
        SummonChaosInsurgencyVan = 128,

        /// <summary>
        /// Summons the NTF chopper.
        /// </summary>
        SummonNtfChopper = 129,
    }
}
