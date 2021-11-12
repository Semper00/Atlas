using Atlas.Entities;
using Atlas.EventSystem;

using PlayableScps;

namespace Atlas.Events
{
    /// <summary>
    /// Fires before SCP-096 "tries not to cry".
    /// </summary>
    public class TryingNotToCry : BoolEvent
    {
        /// <summary>
        /// Gets the SCP-096 instance.
        /// </summary>
        public Scp096 Scp { get; }

        /// <summary>
        /// Gets the player who is controlling SCP-096.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the <see cref="Entities.Door"/> to be cried on.
        /// </summary>
        public Door Door { get; }

        public TryingNotToCry(Scp096 scp, Player player, Door door, bool allow)
        {
            Scp = scp;
            Player = player;
            Door = door;
            IsAllowed = allow;
        }
    }
}
