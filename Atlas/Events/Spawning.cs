using Atlas.Entities;
using Atlas.EventSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Fires before a player spawns.
    /// </summary>
    public class Spawning : BoolEvent
    {
        /// <summary>
        /// Gets or sets the role the player will spawn as.
        /// </summary>
        public RoleType Role { get; set; }

        /// <summary>
        /// Gets or sets the Y axis of the player's rotation.
        /// </summary>
        public float RotationY { get; set; }

        /// <summary>
        /// Gets the player that is spawning.
        /// </summary>
        public Player Player { get; }

        public Spawning(RoleType role, float rotY, Player player, bool allow)
        {
            Role = role;
            RotationY = rotY;
            Player = player;
            IsAllowed = allow;
        }
    }
}
