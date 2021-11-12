using Atlas.Entities;
using Atlas.EventSystem;

using UnityEngine;

namespace Atlas.Events
{
    /// <summary>
    /// Fires before a player enters the pocket dimension.
    /// </summary>
    public class EnteringPocketDimension : BoolEvent
    {
        /// <summary>
        /// Gets the player entering the pocket dimension.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the SCP-106 who sent the player to the pocket dimension.
        /// </summary>
        public Player Scp { get; }

        /// <summary>
        /// Gets or sets the pocket dimension's position.
        /// </summary>
        public Vector3 Position { get; set; }

        public EnteringPocketDimension(Player player, Player scp, Vector3 pos, bool allow)
        {
            Player = player;
            Scp = scp;
            Position = pos;
            IsAllowed = allow;
        }
    }
}
