using InventorySystem.Items.ThrowableProjectiles;

using UnityEngine;

namespace Atlas.Entities.Pickups.Base
{
    /// <summary>
    /// The base for grenades.
    /// </summary>
    public class BaseGrenade : BaseProjectile
    {
        internal TimeGrenade nade;

        public BaseGrenade(ItemType type) : this(ReferenceHub.HostHub.inventory.CreateItemInstance(type, false).PickupDropModel as TimeGrenade, true) { }

        public BaseGrenade(TimeGrenade nade, bool addToApi = false) : base(nade, addToApi)
            => this.nade = nade;

        /// <summary>
        /// Gets the target time.
        /// </summary>
        public float TargetTime { get => nade.TargetTime; set => nade.TargetTime = value; }

        /// <summary>
        /// Gets the nade's fuse time.
        /// </summary>
        public float FuseTime { get => nade._fuseTime; set => nade._fuseTime = value; }

        /// <summary>
        /// Gets a value indicating whether the nade has detonated or not.
        /// </summary>
        public bool IsDetonated { get => nade._alreadyDetonated;  }

        /// <summary>
        /// Ends the nade's fuse.
        /// </summary>
        public void EndFuse()
            => nade.ServerFuseEnd();

        /// <summary>
        /// Spawns an active grenade.
        /// </summary>
        /// <param name="pos">The position to spawn at.</param>
        /// <param name="fuseTime">The grenade's fuse time.</param>
        /// <returns>The spawned grenade.</returns>
        public BaseGrenade SpawnActive(Vector3 pos, float fuseTime)
        {
            FuseTime = fuseTime;

            Spawn(pos);

            return this;
        }

        /// <summary>
        /// Spawns an active grenade.
        /// </summary>
        /// <param name="pos">The position to spawn at.</param>
        /// <param name="fuseTime">The grenade's fuse time.</param>
        /// <returns>The spawned grenade.</returns>
        public TGrenade SpawnActive<TGrenade>(Vector3 pos, float fuseTime) where TGrenade : BaseGrenade
        {
            FuseTime = fuseTime;

            Spawn(pos);

            return this as TGrenade;
        }
    }
}