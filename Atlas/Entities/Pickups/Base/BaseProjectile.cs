using InventorySystem.Items.ThrowableProjectiles;

namespace Atlas.Entities.Pickups.Base
{
    /// <summary>
    /// The base for grenades.
    /// </summary>
    public class BaseProjectile : BasePickup
    {
        internal ThrownProjectile projectile;

        internal BaseProjectile() { }

        public BaseProjectile(ItemType type)
            => projectile = ReferenceHub.HostHub.inventory.CreateItemInstance(type, false).PickupDropModel as ThrownProjectile;

        internal BaseProjectile(ThrownProjectile projectile, bool addToApi = false) : base(projectile, addToApi)
            => this.projectile = projectile;

        /// <summary>
        /// Gets or sets the projectile's gravity.
        /// </summary>
        public float Gravity { get => projectile.AdditionalGravity; set => projectile.AdditionalGravity = value; }

        /// <summary>
        /// Activates this projectile.
        /// </summary>
        public void Activate()
           => projectile.ServerActivate();
    }
}