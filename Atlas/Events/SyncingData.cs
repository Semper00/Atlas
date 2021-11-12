using UnityEngine;

using Atlas.Entities;
using Atlas.EventSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Fires before a player syncs his data.
    /// </summary>
    public class SyncingData : BoolEvent
    {
        /// <summary>
        /// Gets the player of the syncing data.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the player's speed.
        /// </summary>
        public Vector2 Speed { get; }

        /// <summary>
        /// Gets or sets the current player's animation.
        /// </summary>
        public Enums.Animation Animation { get; set; }

        public SyncingData(Player player, Vector2 speed, byte currentAnimation, bool allow)
        {
            Player = player;
            Speed = speed;
            Animation = (Enums.Animation)currentAnimation;
            IsAllowed = allow;
        }
    }
}
