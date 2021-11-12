using Atlas.Entities;
using Atlas.EventSystem;

using PlayableScps;

namespace Atlas.Events
{
    /// <summary>
    /// Fires before SCP-096 charges.
    /// </summary>
    public class Scp096Charging : BoolEvent
    {
        /// <summary>
        /// Gets the player controlling SCP-096.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the SCP-096 instance.
        /// </summary>
        public Scp096 Scp { get; }

        public Scp096Charging(Player player, Scp096 scp, bool allow)
        {
            Player = player;
            Scp = scp;
            IsAllowed = allow;
        }
    }
}