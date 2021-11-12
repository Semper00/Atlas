using Atlas.EventSystem;
using Atlas.Entities.Pickups.Base;

namespace Atlas.Events
{
    /// <summary>
    /// Fires when the server is turning a pickup into a live grenade.
    /// </summary>
    public class ChangingIntoGrenade : BoolEvent
    {
        /// <summary>
        /// Gets a value indicating the pickup being changed.
        /// </summary>
        public BasePickup Pickup { get; }

        /// <summary>
        /// Gets or sets a value indicating what type of grenade will be spawned.
        /// </summary>
        public ItemType TargetType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating how long the fuse of the changed grenade will be.
        /// </summary>
        public float FuseTime { get; set; }

        public ChangingIntoGrenade(BasePickup pickup, ItemType target, float fuse, bool allow)
        {
            Pickup = pickup;
            TargetType = target;
            FuseTime = fuse;
            IsAllowed = allow;
        }
    }
}
