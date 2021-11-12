namespace Atlas.DefaultValues
{
    /// <summary>
    /// This class is purely to store default health values for every role (taken from SL's wiki page).
    /// </summary>
    public static class DefaultHealth
    {
        /// <summary>
        /// SCP-049's default health.
        /// </summary>
        public const int SCP_049 = 2500;

        /// <summary>
        /// SCP-049-2's default health.
        /// </summary>
        public const int SCP_049_2 = 500;

        /// <summary>
        /// SCP-079's default health.
        /// </summary>
        public const int SCP_079 = 100000;

        /// <summary>
        /// SCP-096's default health.
        /// </summary>
        public const int SCP_096 = 2000;

        /// <summary>
        /// SCP-106's default health.
        /// </summary>
        public const int SCP_106 = 850;

        /// <summary>
        /// SCP-173's default health.
        /// </summary>
        public const int SCP_173 = 3000;

        /// <summary>
        /// SCP-939-89's default health.
        /// </summary>
        public const int SCP_939_89 = 1800;

        /// <summary>
        /// SCP-939-53's default health.
        /// </summary>
        public const int SCP_939_53 = 1800;

        /// <summary>
        /// Chaos Conscript's default health.
        /// </summary>
        public const int CHAOS_CONSCRIPT = 100;

        /// <summary>
        /// Chaos Rifleman's default health.
        /// </summary>
        public const int CHAOS_RIFLEMAN = 100;

        /// <summary>
        /// Chaos Repressor's default health.
        /// </summary>
        public const int CHAOS_REPRESSOR = 100;

        /// <summary>
        /// Chaos Marauder's default health.
        /// </summary>
        public const int CHAOS_MARAUDER = 100;

        /// <summary>
        /// Class-D Personnel's default health.
        /// </summary>
        public const int CLASS_D = 100;

        /// <summary>
        /// Scientist's default health.
        /// </summary>
        public const int SCIENTIST = 100;

        /// <summary>
        /// Facility Guard's default health.
        /// </summary>
        public const int FACILITY_GUARD = 100;

        /// <summary>
        /// Spectator's default health.
        /// </summary>
        public const int SPECTATOR = 100;

        /// <summary>
        /// NTF Specialist's default health.
        /// </summary>
        public const int NTF_SPECIALIST = 100;

        /// <summary>
        /// NTF Sergeant's default health.
        /// </summary>
        public const int NTF_SERGEANT = 100;

        /// <summary>
        /// NTF Captain's default health.
        /// </summary>
        public const int NTF_CAPTAIN = 100;

        /// <summary>
        /// NTF Private's default health.
        /// </summary>
        public const int NTF_PRIVATE = 100;

        /// <summary>
        /// Tutorial's default health.
        /// </summary>
        public const int TUTORIAL = 100;

        /// <summary>
        /// <see cref="RoleType.None"/>
        /// </summary>
        public const int NONE = -1;

        /// <summary>
        /// Gets a default health integer for the specified role.
        /// </summary>
        /// <param name="role">The role to retrieve health of.</param>
        /// <returns>The role's default health.</returns>
        public static int Get(RoleType role)
        {
            switch (role)
            {
                case RoleType.ChaosConscript:
                    return CHAOS_CONSCRIPT;
                case RoleType.ChaosMarauder:
                    return CHAOS_MARAUDER;
                case RoleType.ChaosRepressor:
                    return CHAOS_REPRESSOR;
                case RoleType.ChaosRifleman:
                    return CHAOS_RIFLEMAN;
                case RoleType.ClassD:
                    return CLASS_D;
                case RoleType.FacilityGuard:
                    return FACILITY_GUARD;
                case RoleType.None:
                    return NONE;
                case RoleType.NtfCaptain:
                    return NTF_CAPTAIN;
                case RoleType.NtfPrivate:
                    return NTF_PRIVATE;
                case RoleType.NtfSergeant:
                    return NTF_SERGEANT;
                case RoleType.NtfSpecialist:
                    return NTF_SPECIALIST;
                case RoleType.Scientist:
                    return SCIENTIST;
                case RoleType.Scp049:
                    return SCP_049;
                case RoleType.Scp0492:
                    return SCP_049_2;
                case RoleType.Scp079:
                    return SCP_079;
                case RoleType.Scp096:
                    return SCP_096;
                case RoleType.Scp106:
                    return SCP_106;
                case RoleType.Scp173:
                    return SCP_173;
                case RoleType.Scp93953:
                    return SCP_939_53;
                case RoleType.Scp93989:
                    return SCP_939_89;
                case RoleType.Spectator:
                    return SPECTATOR;
                case RoleType.Tutorial:
                    return TUTORIAL;
                default:
                    return 100;
            }
        }
    }
}
