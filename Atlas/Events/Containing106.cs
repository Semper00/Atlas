using Atlas.EventSystem;
using Atlas.Entities;

namespace Atlas.Events
{
    /// <summary>
    /// Fires before SCP-106 gets contained.
    /// </summary>
    public class Containing106 : BoolEvent
    {
        /// <summary>
        /// Gets the player controlling SCP-106.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the player that pressed the containment button.
        /// </summary>
        public Player Killer { get; }

        public Containing106(Player scp, Player player, bool allow)
        {
            Player = scp;
            Killer = player;
            IsAllowed = allow;
        }
    }
}
