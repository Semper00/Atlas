using Atlas.Entities;
using Atlas.EventSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Fires before SCP-079 gains a new level.
    /// </summary>
    public class GainingLevel : BoolEvent
    {
        /// <summary>
        /// Gets the player controlling SCP-079.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the <see cref="Scp079PlayerScript"/> instance.
        /// </summary>
        public Scp079PlayerScript Scp { get; }

        /// <summary>
        /// Gets or sets the new level.
        /// </summary>
        public int Level { get; set; }

        public GainingLevel(Player player, Scp079PlayerScript scp, int lvl, bool allow)
        {
            Player = player;
            Scp = scp;
            Level = lvl;
            IsAllowed = allow;
        }
    }
}
