using Atlas.Entities;
using Atlas.Entities.Items.Base;
using Atlas.EventSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Fires after the player has used a usable item.
    /// </summary>
    public class UsedItem : Event
    {
        /// <summary>
        /// Gets the player who used the medical item.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the usable item that the player consumed.
        /// </summary>
        public BaseUsableItem Item { get; }

        public UsedItem(Player player, BaseUsableItem item)
        {
            Player = player;
            Item = item;
        }
    }
}
