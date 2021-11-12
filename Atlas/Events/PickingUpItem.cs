using Atlas.Entities;
using Atlas.Entities.Pickups.Base;
using Atlas.EventSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Fires before a player picks up an item.
    /// </summary>
    public class PickingUpItem : BoolEvent
    {
        /// <summary>
        /// Gets the player who's picking up this item.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the item's that is being picked up.
        /// </summary>
        public BasePickup Item { get; }

        public PickingUpItem(Player player, BasePickup pickup, bool allow)
        {
            Player = player;
            Item = pickup;
            IsAllowed = allow;
        }
    }
}
