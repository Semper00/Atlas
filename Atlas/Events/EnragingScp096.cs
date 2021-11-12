using Atlas.Entities;
using Atlas.EventSystem;

using PlayableScps;

namespace Atlas.Events
{
    /// <summary>
    /// Fires before SCP-096 starts enraging.
    /// </summary>
    public class EnragingScp096 : BoolEvent
    {
        /// <summary>
        /// Gets the player controlling SCP-096.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the SCP-096 instance.
        /// </summary>
        public Scp096 Scp { get; }

        public EnragingScp096(Player player, Scp096 scp, bool allow)
        {
            Player = player;
            Scp = scp;
            IsAllowed = allow;
        }
    }
}
