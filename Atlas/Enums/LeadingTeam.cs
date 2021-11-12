namespace Atlas.Enums
{
    /// <summary>
    /// The team that is currently leading the round.
    /// </summary>
    public enum LeadingTeam : byte
    {
        /// <summary>
        /// Represents Scientists, Guards, and NTF.
        /// </summary>
        FacilityForces,

        /// <summary>
        /// Represents Class-D and Chaos Insurgency.
        /// </summary>
        ChaosInsurgency,

        /// <summary>
        /// Represents SCPs.
        /// </summary>
        Anomalies,

        /// <summary>
        /// Represents a draw.
        /// </summary>
        Draw,
    }
}
