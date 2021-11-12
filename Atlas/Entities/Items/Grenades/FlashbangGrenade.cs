using Atlas.Entities.Pickups.Base;

namespace Atlas.Entities.Items.Grenades
{
    /// <summary>
    /// Represents the in-game flashbang.
    /// </summary>
    public class FlashbangGrenade : BaseGrenade
    {
        internal new InventorySystem.Items.ThrowableProjectiles.FlashbangGrenade nade;

        public FlashbangGrenade(InventorySystem.Items.ThrowableProjectiles.FlashbangGrenade nade, bool addToApi = false) : base(nade, addToApi)
            => this.nade = nade;

        /// <summary>
        /// Gets or sets the grenade's blur effect additional duration.
        /// </summary>
        public float BlurDuration { get => nade._additionalBlurDuration; set => nade._additionalBlurDuration = value; }

        /// <summary>
        /// Gets or sets the grenade's surface distance intensifier.
        /// </summary>
        public float SufaceDistanceIntensifier { get => nade._surfaceZoneDistanceIntensifier; set => nade._surfaceZoneDistanceIntensifier = value; }

        /// <summary>
        /// Explodes this grenade.
        /// </summary>
        public void Explode()
            => nade.PlayExplosionEffects();

        /// <summary>
        /// Tries to flash this player.
        /// </summary>
        /// <param name="player">The player to flash.</param>
        public void Flash(Player player)
            => nade.ProcessPlayer(player.Hub);
    }
}
