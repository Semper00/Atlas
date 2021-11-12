using Atlas.Entities.Pickups.Base;
using Atlas.Entities;
using Atlas.EventSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Fires before an item spawns.
    /// </summary>
    public class SpawningItem : BoolEvent
    {
        /// <summary>
        /// Gets or sets the item that will be spawned.
        /// </summary>
        public BasePickup Pickup { get; set; }

        /// <summary>
        /// Gets the item's owner.
        /// </summary>
        public Player Owner { get => Pickup?.Owner ?? PlayersList.host; }

        public SpawningItem(BasePickup pickup, bool allow)
        {
            Pickup = pickup;
            IsAllowed = allow;
        }
    }
}
