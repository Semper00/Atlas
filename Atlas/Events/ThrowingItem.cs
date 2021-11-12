using Atlas.Entities;
using Atlas.EventSystem;
using Atlas.Enums;

using InventorySystem.Items.ThrowableProjectiles;


namespace Atlas.Events
{
    public class ThrowingItem : BoolEvent
    { 
        /// <summary>
        /// Gets the player who's throwing the grenade.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets or sets the item being thrown.
        /// </summary>
        public Entities.Items.ThrowableItem Item { get; set; }

        /// <summary>
        ///  Gets or sets the type of throw being requested.
        /// </summary>
        public ThrowRequest RequestType { get; set; }

        public ThrowingItem(Player player, ThrowableNetworkHandler.RequestType request, bool allow)
        {
            Player = player;
            Item = player.CurrentItem is Entities.Items.ThrowableItem throwable ? throwable : null;
            RequestType = (ThrowRequest) request;
            IsAllowed = allow;
        }
    }
}
