namespace Atlas.Entities
{
    /// <summary>
    /// A better version of the <see cref="RoundSummary.SumInfo_ClassList"/>.
    /// </summary>
    public struct ClassList
    {
        public ClassList(RoundSummary.SumInfo_ClassList list)
        {
            ChaosInsurgents = list.chaos_insurgents;
            ClassDs = list.class_ds;
            MTF = list.mtf_and_guards;
            Scientists = list.scientists;
            SCPs = list.scps_except_zombies;
            Zombies = list.zombies;
            Time = list.time;
            WarheadKills = list.warhead_kills;
        }

        /// <summary>
        /// Gets or sets the amount of chaos insurgents.
        /// </summary>
        public int ChaosInsurgents { get; set; }

        /// <summary>
        /// Gets or sets the amount of Class-Ds.
        /// </summary>
        public int ClassDs { get; set; }

        /// <summary>
        /// Gets or sets the amount of MTF (facility guards included).
        /// </summary>
        public int MTF { get; set; }

        /// <summary>
        /// Gets or sets the amount of scientists.
        /// </summary>
        public int Scientists { get; set; }

        /// <summary>
        /// Gets or sets the amount of SCPs (zombies excluded).
        /// </summary>
        public int SCPs { get; set; }

        /// <summary>
        /// Gets or sets the amount of zombies.
        /// </summary>
        public int Zombies { get; set; }

        /// <summary>
        /// Gets or sets the round's duration (in seconds).
        /// </summary>
        public int Time { get; set; }

        /// <summary>
        /// Gets or sets the amount of warhead kills.
        /// </summary>
        public int WarheadKills { get; set; }

        /// <summary>
        /// Converts this instance to a <see cref="RoundSummary.SumInfo_ClassList"/>.
        /// </summary>
        /// <returns>The converted instance.</returns>
        public RoundSummary.SumInfo_ClassList ToBase()
            => new RoundSummary.SumInfo_ClassList
            {
                chaos_insurgents = ChaosInsurgents,
                class_ds = ClassDs,
                scps_except_zombies = SCPs,
                mtf_and_guards = MTF,
                scientists = Scientists,
                time = Time,
                warhead_kills = WarheadKills,
                zombies = Zombies
            };
    }
}
