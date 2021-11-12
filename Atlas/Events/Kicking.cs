using Atlas.Entities;
using Atlas.EventSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Fires after a player gets kicked.
    /// </summary>
    public class Kicking : BoolEvent
    {
        /// <summary>
        /// Gets the player that kicked the target player.
        /// </summary>
        public Player Admin { get; }

        /// <summary>
        /// Gets the player who's being kicked.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets or sets the kick reason.
        /// </summary>
        public string Reason { get; set; }

        public Kicking(Player admin, Player player, string reason)
        {
            Admin = admin;
            Player = player;
            Reason = reason;
            IsAllowed = true;
        }
    }
}