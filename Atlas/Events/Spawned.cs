using Atlas.Entities;
using Atlas.EventSystem;

using UnityEngine;

namespace Atlas.Events
{
    /// <summary>
    /// Fires after a player fully spawns.
    /// </summary>
    public class Spawned : Event
    {
        /// <summary>
        /// Gets the player who spawned.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the role the player spawned as.
        /// </summary>
        public RoleType Role { get; }

        /// <summary>
        /// Gets the position the player spawned at.
        /// </summary>
        public Vector3 Position { get; }

        public Spawned(Player player, RoleType role, Vector3 pos)
        {
            Player = player;
            Role = role;
            Position = pos;
        }
    }
}
