using Atlas.Entities;
using Atlas.EventSystem;

using UnityEngine;

namespace Atlas.Events
{
    /// <summary>
    /// Fires before SCP-106 creates a portal.
    /// </summary>
    public class CreatingPortal106 : BoolEvent
    {
        /// <summary>
        /// Gets the player who's controlling SCP-106.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets or sets the portal's position.
        /// </summary>
        public Vector3 Position { get; set; } 

        public CreatingPortal106(Player player, Vector3 pos, bool allow)
        {
            Player = player;
            Position = pos;
            IsAllowed = allow;
        }
    }
}
