using System.Collections.Generic;

namespace Atlas.DefaultValues
{
    /// <summary>
    /// This class is purely to store default items for every role in the correct order (taken from SL's wiki page).
    /// </summary>
    public static class DefaultItems
    {
        /// <summary>
        /// SCP-173's default items.
        /// </summary>
        public static readonly IReadOnlyList<ItemType> SCP_173 = new List<ItemType>();

        /// <summary>
        /// SCP-096's default items.
        /// </summary>
        public static readonly IReadOnlyList<ItemType> SCP_096 = new List<ItemType>();

        /// <summary>
        /// SCP-079's default items.
        /// </summary>
        public static readonly IReadOnlyList<ItemType> SCP_079 = new List<ItemType>();

        /// <summary>
        /// SCP-049's default items.
        /// </summary>
        public static readonly IReadOnlyList<ItemType> SCP_049 = new List<ItemType>();

        /// <summary>
        /// SCP-049-2's default items.
        /// </summary>
        public static readonly IReadOnlyList<ItemType> SCP_049_2 = new List<ItemType>();

        /// <summary>
        /// SCP-939-89's default items.
        /// </summary>
        public static readonly IReadOnlyList<ItemType> SCP_939_89 = new List<ItemType>();

        /// <summary>
        /// SCP-939-53's default items.
        /// </summary>
        public static readonly IReadOnlyList<ItemType> SCP_939_53 = new List<ItemType>();

        /// <summary>
        /// SCP-106's default items.
        /// </summary>
        public static readonly IReadOnlyList<ItemType> SCP_106 = new List<ItemType>();

        /// <summary>
        /// NTF Captain's default items.
        /// </summary>
        public static readonly IReadOnlyList<ItemType> NTF_CAPTAIN = new List<ItemType>()
        {
            ItemType.KeycardNTFCommander,
            ItemType.GunE11SR,
            ItemType.ArmorHeavy,
            ItemType.GrenadeHE,
            ItemType.Radio,
            ItemType.Adrenaline,
            ItemType.Medkit
        };

        /// <summary>
        /// NTF Sergeant's default items.
        /// </summary>
        public static readonly IReadOnlyList<ItemType> NTF_SERGEANT = new List<ItemType>()
        {
            ItemType.KeycardNTFLieutenant,
            ItemType.GunE11SR,
            ItemType.ArmorCombat,
            ItemType.GrenadeHE,
            ItemType.Radio,
            ItemType.Medkit
        };

        /// <summary>
        /// NTF Specialist's default items.
        /// </summary>
        public static readonly IReadOnlyList<ItemType> NTF_SPECIALIST = new List<ItemType>()
        {
            ItemType.KeycardNTFLieutenant,
            ItemType.GunE11SR,
            ItemType.ArmorCombat,
            ItemType.GrenadeHE,
            ItemType.Radio,
            ItemType.Medkit,
            ItemType.Medkit
        };

        /// <summary>
        /// NTF Private's default items.
        /// </summary>
        public static readonly IReadOnlyList<ItemType> NTF_PRIVATE = new List<ItemType>()
        {
            ItemType.KeycardNTFOfficer,
            ItemType.GunCrossvec,
            ItemType.ArmorCombat,
            ItemType.Radio,
            ItemType.Medkit
        };

        /// <summary>
        /// Chaos Riflemen's default items.
        /// </summary>
        public static readonly IReadOnlyList<ItemType> CHAOS_RIFLEMEN = new List<ItemType>()
        {
            ItemType.KeycardChaosInsurgency,
            ItemType.GunAK,
            ItemType.Medkit,
            ItemType.Painkillers,
            ItemType.ArmorCombat,
        };

        /// <summary>
        /// Chaos Marauder's default items.
        /// </summary>
        public static readonly IReadOnlyList<ItemType> CHAOS_MARAUDER = new List<ItemType>()
        {
            ItemType.KeycardChaosInsurgency,
            ItemType.GunRevolver,
            ItemType.GunShotgun,
            ItemType.Medkit,
            ItemType.Painkillers,
            ItemType.ArmorCombat,
        };

        /// <summary>
        /// Chaos Repressor's default items.
        /// </summary>
        public static readonly IReadOnlyList<ItemType> CHAOS_REPRESSOR = new List<ItemType>()
        {
            ItemType.KeycardChaosInsurgency,
            ItemType.GunLogicer,
            ItemType.Medkit,
            ItemType.Adrenaline,
            ItemType.ArmorHeavy,
        };

        /// <summary>
        /// Chaos Conscript's default items.
        /// </summary>
        public static readonly IReadOnlyList<ItemType> CHAOS_CONSCRIPT = new List<ItemType>()
        {
            ItemType.KeycardGuard,
            ItemType.GunFSP9,
            ItemType.GrenadeFlash,
            ItemType.Radio,
            ItemType.Medkit,
            ItemType.ArmorLight
        };

        /// <summary>
        /// Facility Guard's default items.
        /// </summary>
        public static readonly IReadOnlyList<ItemType> FACILITY_GUARD = new List<ItemType>()
        {
            ItemType.KeycardChaosInsurgency,
            ItemType.GunAK,
            ItemType.Medkit,
            ItemType.Painkillers,
            ItemType.ArmorCombat,
        };

        /// <summary>
        /// Scientist's default items.
        /// </summary>
        public static readonly IReadOnlyList<ItemType> SCIENTIST = new List<ItemType>()
        {
            ItemType.KeycardScientist,
            ItemType.Medkit,
        };

        /// <summary>
        /// Class-D's default items.
        /// </summary>
        public static readonly IReadOnlyList<ItemType> CLASS_D = new List<ItemType>();

        /// <summary>
        /// <see cref="RoleType.None"/>
        /// </summary>
        public static readonly IReadOnlyList<ItemType> NONE = new List<ItemType>();

        /// <summary>
        /// Spectator's default items.
        /// </summary>
        public static readonly IReadOnlyList<ItemType> SPECTATOR = new List<ItemType>();

        /// <summary>
        /// Tutorial's default items.
        /// </summary>
        public static readonly IReadOnlyList<ItemType> TUTORIAL = new List<ItemType>();

        /// <summary>
        /// Gets a default item list for the specified role.
        /// </summary>
        /// <param name="role">The role to retrieve items of.</param>
        /// <returns>The role's default items.</returns>
        public static IReadOnlyList<ItemType> Get(RoleType role)
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
                    return CHAOS_RIFLEMEN;
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
                    return new List<ItemType>();
            }
        }
    }
}
