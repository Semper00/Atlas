using InventorySystem.Items.ThrowableProjectiles;

using UnityEngine;

using Atlas.Entities.Pickups.Base;

namespace Atlas.Entities.Items.Grenades
{
    /// <summary>
    /// Represents the <see cref="ItemType.GrenadeHE"/>.
    /// </summary>
    public class ExplosiveGrenade : BaseGrenade
    {
        internal new ExplosionGrenade nade;

        public ExplosiveGrenade(ExplosionGrenade nade, bool addToApi = false) : base(nade, addToApi)
            => this.nade = nade;

        /// <summary>
        /// Gets the grenade's armor penetration factor.
        /// </summary>
        public float Penetration { get => nade.ArmorPenetration; }

        /// <summary>
        /// Gets the grenade's damage type.
        /// </summary>
        public DamageTypes.DamageType DamageType { get => nade.DamageType; }

        /// <summary>
        /// Explodes this grenade.
        /// </summary>
        public void Explode()
            => nade.Explode();

        /// <summary>
        /// Destroys the <see cref="IDestructible"/>.
        /// </summary>
        /// <param name="destructible">The <see cref="IDestructible"/> to destroy.</param>
        public void Destroy(IDestructible destructible)
            => nade.ExplodeDestructible(destructible);

        /// <summary>
        /// Destroys the <see cref="Door"/>.
        /// </summary>
        /// <param name="door">The door to destroy.</param>
        public void Destroy(Door door)
            => nade.ExplodeDoor(door.Base);

        /// <summary>
        /// Adds a force to the <see cref="Rigidbody"/>.
        /// </summary>
        /// <param name="rigidbody">The rigidbody to move.</param>
        public void Explode(Rigidbody rigidbody)
            => nade.ExplodeRigidbody(rigidbody);
    }
}
