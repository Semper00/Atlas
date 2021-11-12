using Atlas.Entities;
using Atlas.EventSystem;

using PlayableScps;

namespace Atlas.Events
{
    /// <summary>
    /// Fires before SCP-049 revives a ragdoll.
    /// </summary>
    public class FinishingRecall : BoolEvent
    {
        /// <summary>
        /// Gets the player that is being revived.
        /// </summary>
        public Player Target { get; }

        /// <summary>
        /// Gets the player who is controlling SCP-049.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the SCP-049 instance.
        /// </summary>
        public Scp049 Scp { get; }

        /// <summary>
        /// Gets the ragdoll that is being revived.
        /// </summary>
        public Ragdoll Ragdoll { get; }

        public FinishingRecall(Player target, Player player, Scp049 scp, Ragdoll ragdoll, bool allow)
        {
            Target = target;
            Player = player;
            Scp = scp;
            Ragdoll = ragdoll;
            IsAllowed = allow;
        }
    }
}
