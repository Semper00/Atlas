namespace Atlas.Enums
{
    /// <summary>
    /// Ammo types present in the game.
    /// </summary>
    public enum AmmoType
    {
        /// <summary>
        /// Not ammo.
        /// </summary>
        None,

        /// <summary>
        /// 5.56mm Ammunition.
        /// Used by <see cref="ItemType.GunE11Sr"/>.
        /// </summary>
        Ammo556,

        /// <summary>
        /// 7.62mm Ammunition.
        /// Used by and <see cref="ItemType.GunLogicer"/>.
        /// </summary>
        Ammo762,

        /// <summary>
        /// 9mm Ammunition.
        /// Used by <see cref="ItemType.GunCom15"/>,.
        /// </summary>
        Ammo9,

        /// <summary>
        /// 12 gauge shotgun ammo.
        /// Used by <see cref="ItemType.GunShotgun"/>
        /// </summary>
        Ammo12Gauge,

        /// <summary>
        /// 44 caliber.
        /// </summary>
        Ammo44Cal,
    }
}
