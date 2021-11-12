using Atlas.Entities;
using Atlas.EventSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Fires before a player escapes.
    /// </summary>
    public class Escaping : BoolEvent
    {
        /// <summary>
        /// Gets the player that escaped.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets or the player's new role.
        /// </summary>
        public RoleType Role { get; set; }

        public Escaping(Player player, RoleType role, bool allow)
        {
            Player = player;
            Role = role;
            IsAllowed = allow;
        }
    }
}
