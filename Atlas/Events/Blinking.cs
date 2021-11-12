using Atlas.Entities;
using Atlas.EventSystem;

using PlayableScps;


namespace Atlas.Events
{
    /// <summary>
    /// Fires when SCP-173 forces players to blink.
    /// </summary>
    public class Blinking : BoolEvent
    {
        /// <summary>
        /// Gets the player playing as SCP-173.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the SCP-173 instance.
        /// </summary>
        public Scp173 Scp { get; }

        public Blinking(Player ply, Scp173 scp, bool allow)
        {
            Player = ply;
            Scp = scp;
            IsAllowed = allow;
        }
    }
}
