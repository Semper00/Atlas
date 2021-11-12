using Atlas.Entities;
using Atlas.EventSystem;

using PlayableScps;

namespace Atlas.Events
{
    /// <summary>
    /// Fires before SCP-096 receives a new target.
    /// </summary>
    public class AddingTargetScp096 : BoolEvent
    {
        /// <summary>
        /// Gets the SCP-096 instance.
        /// </summary>
        public Scp096 Scp { get; }

        /// <summary>
        /// Gets the player controlling SCP-096.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the player that looked at SCP-096.
        /// </summary>
        public Player Target { get; }

        public AddingTargetScp096(Player player, Player target, Scp096 scp, bool allow)
        {
            Scp = scp;
            Player = player;
            Target = target;
            IsAllowed = allow;
        }
    }
}
