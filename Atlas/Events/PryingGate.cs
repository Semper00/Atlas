using Atlas.EventSystem;
using Atlas.Entities;

using PlayableScps;

namespace Atlas.Events
{
    /// <summary>
    /// Fires before SCP-096 pryes a gate.
    /// </summary>
    public class PryingGate : BoolEvent
    {
        /// <summary>
        /// Gets the player who's controlling SCP-096.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the SCP-096 instance.
        /// </summary>
        public Scp096 Scp { get; }

        /// <summary>
        /// Gets the gate.
        /// </summary>
        public Door Door { get; }

        public PryingGate(Player player, Scp096 scp, Door door, bool allow)
        {
            Player = player;
            Scp = scp;
            Door = door;
            IsAllowed = allow;
        }
    }
}
