using Atlas.Entities;
using Atlas.EventSystem;

using System.Collections.Generic;

namespace Atlas.Events
{
    /// <summary>
    /// Fires when a player changes roles.
    /// </summary>
    public class ChangingRole : BoolEvent
    {
        /// <summary>
        /// Gets the player that changed role.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets or sets the new role.
        /// </summary>
        public RoleType Role { get; set; }

        /// <summary>
        /// Gets or sets the list of items the player will spawn with.
        /// </summary>
        public List<ItemType> Items { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the player has escaped or not.
        /// </summary>
        public bool IsEscaped { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to save position.
        /// </summary>
        public bool SavePosition { get; set; }

        public ChangingRole(Player player, RoleType role, List<ItemType> items, bool escaped, bool position, bool allow)
        {
            Player = player;
            Role = role;
            Items = items;
            IsEscaped = escaped;
            SavePosition = position;
            IsAllowed = allow;
        }
    }
}
