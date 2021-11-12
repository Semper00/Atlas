using Atlas.Entities;
using Atlas.EventSystem;

using UnityEngine;

namespace Atlas.Events
{
    /// <summary>
    /// Fires before a player escapes from the pocket dimension.
    /// </summary>
    public class EscapingPocketDimension : BoolEvent
    {
        /// <summary>
        /// Gets the escaping player.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets or sets the position that the player is going to be teleport to.
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Gets the pocket dimension exit the player has walked into.
        /// </summary>
        public PocketDimensionExit Exit { get; }

        public EscapingPocketDimension(Player player, Vector3 pos, PocketDimensionExit ex, bool allow)
        {
            Player = player;
            Position = pos;
            Exit = ex;
            IsAllowed = allow;
        }
    }
}
