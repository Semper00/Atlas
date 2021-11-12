using Atlas.Entities;
using Atlas.Entities.Pickups.Base;

using Atlas.EventSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Fires after a player drops an item.
    /// </summary>
    public class DroppedItem : Event
    {
        /// <summary>
        /// Gets the player who dropped the item.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the item that was dropped.
        /// </summary>
        public BasePickup Item { get; }

        public DroppedItem(Player player, BasePickup item)
        {
            Player = player;
            Item = item;
        }
    }
}
