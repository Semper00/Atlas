using Atlas.EventSystem;
using Atlas.Entities;

namespace Atlas.Events
{
    /// <summary>
    /// Fires when the warhead tries to start.
    /// </summary>
    public class WarheadStarting : BoolEvent
    {
        /// <summary>
        /// Gets the player that started this detonation.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets or sets the remaining time to detonation.
        /// </summary>
        public int TimeToDetonation { get; set; }

        public WarheadStarting(Player player, int time, bool allow)
        {
            Player = player;
            TimeToDetonation = time;
            IsAllowed = allow;
        }
    }
}
