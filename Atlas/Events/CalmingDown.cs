using Atlas.Entities;
using Atlas.EventSystem;

using PlayableScps;

namespace Atlas.Events
{
    /// <summary>
    /// Fires when SCP-096 starts calming down.
    /// </summary>
    public class CalmingDown : BoolEvent
    {
        /// <summary>
        /// Gets the player playing as SCP-096.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the SCP-096 instance.
        /// </summary>
        public Scp096 Scp { get; }

        public CalmingDown(Player ply, Scp096 scp, bool allow)
        {
            Player = ply;
            Scp = scp;
            IsAllowed = allow;
        }
    }
}
