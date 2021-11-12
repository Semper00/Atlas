using UnityEngine;

using Atlas.Entities.Items.Base;

namespace Atlas.Entities.Items
{
    /// <summary>
    /// Represents a throwable item.
    /// </summary>
    public class ThrowableItem : BaseItem
    {
        internal InventorySystem.Items.ThrowableProjectiles.ThrowableItem item;

        public ThrowableItem(InventorySystem.Items.ThrowableProjectiles.ThrowableItem item, bool addToApi = false) : base(item, addToApi)
            => this.item = item;

        /// <summary>
        /// Gets the <see cref="InventorySystem.Items.ThrowableProjectiles.ThrowableItem.ProjectileSettings"/> for a full force throw.
        /// </summary>
        public InventorySystem.Items.ThrowableProjectiles.ThrowableItem.ProjectileSettings FullThrowSettings { get => item.FullThrowSettings; 
            set => item.FullThrowSettings = value; }

        /// <summary>
        /// Gets the <see cref="InventorySystem.Items.ThrowableProjectiles.ThrowableItem.ProjectileSettings"/> for a weak force throw.
        /// </summary>
        public InventorySystem.Items.ThrowableProjectiles.ThrowableItem.ProjectileSettings WeakThrowSettings { get => item.WeakThrowSettings;
            set => item.WeakThrowSettings = value; }

        /// <summary>
        /// Throws the item.
        /// </summary>
        /// <param name="fullForce">Whether or not to throw at full force.</param>
        public void Throw(bool fullForce)
            => item.ServerThrow(fullForce);

        /// <summary>
        /// Throws the item.
        /// </summary>
        /// <param name="force">The force to throw at.</param>
        /// <param name="factor">The item's upwards factor.</param>
        /// <param name="torque">The item's torque.</param>
        public void Throw(float force, float factor, Vector3 torque)
            => item.ServerThrow(force, factor, torque);
    }
}