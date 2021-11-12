namespace Atlas.Enums
{
    /// <summary>
    /// Represents the server branch.
    /// </summary>
    public enum BetaBranch
    {
        /// <summary>
        /// The "pre-beta staging" branch.
        /// </summary>
        Staging,

        /// <summary>
        /// A public beta branch.
        /// </summary>
        Public,

        /// <summary>
        /// This is not a beta but a full server release.
        /// </summary>
        FullRelease
    }
}
