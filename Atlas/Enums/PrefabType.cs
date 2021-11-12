using System;

namespace Atlas.Enums
{
    /// <summary>
    /// I am NOT going to document all of these
    /// </summary>
    public enum PrefabType
    {
        Player,
        PlaybackLobby,

        [Obsolete("Every pickup has it's own prefab since 11.0")]
        Pickup,

        [Obsolete("Replaced by WorkstationStructure")]
        WorkStation,

        Ragdoll_SCP173,
        Ragdoll_1,
        Ragdoll_SCP106,
        Ragdoll_4,
        Ragdoll_6,
        Ragdoll_7,
        Ragdoll_8,
        Ragdoll_SCP096,
        Ragdoll_10,
        Ragdoll_14,
        Ragdoll_SCP939_53,
        Ragdoll_SCP939_89,
        Ragdoll_Tutorial,

        [Obsolete("Replaced by HegProjectile.")]
        GrenadeFlash,

        [Obsolete("Replaced by FlashbangProjectile.")]
        GrenadeFrag,

        [Obsolete("Replaced by Scp018Projectile.")]
        GrenadeSCP018,

        EZ_BreakableDoor,
        HCZ_BreakableDoor,
        LCZ_BreakableDoor,

        SportTarget,
        DboyTarget,
        BinaryTarget,

        Tantrum,

        RegularKeycardPickup,
        ChaosKeycardPickup,

        RadioPickup,
        Com15Pickup,
        MedkitPickup,
        FlashlightPickup,
        MicroHidPickup,
        SCP500Pickup,
        SCP207Pickup,
        Ammo12gaPickup,
        E11SRPickup,
        CrossvecPickup,
        Ammo556mmPickup,
        Fsp9Pickup,
        LogicerPickup,
        HegPickup,
        FlashbangPickup,
        Ammo44calPickup,
        Ammo762mmPickup,
        Ammo9mmPickup,
        Com18Pickup,
        Scp018Projectile,
        SCP268Pickup,
        AdrenalinePrefab,
        PainkillersPickup,
        CoinPickup,
        LightArmorPickup,
        CombatArmorPickup,
        HeavyArmorPickup,
        RevolverPickup,
        AkPickup,
        ShotgunPickup,
        Scp330Pickup,
        MutantHandsPickup,
        Scp2176Projectile,
        Scp268PedestalStructure,
        Scp207PedestalStructure,
        Scp500PedestalStructure,
        Scp018PedestalStructure,
        LargeGunLockerStructure,
        RifleRackStructure,
        MiscLocker,
        GeneratorStructure,
        WorkstationStructure,
        RegularMedkitStructure,
        AdrenalineMedkitStructure,
        Scp2176PedestalStructure,
        HegProjectile,
        FlashbangProjectile,

        Portal106
    }
}
