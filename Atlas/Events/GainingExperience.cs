using Atlas.Entities;
using Atlas.EventSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Fires before SCP-079 gains experience.
    /// </summary>
    public class GainingExperience : BoolEvent
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
        /// Gets or sets the amount of experience.
        /// </summary>
        public float Experience { get; set; }

        public GainingExperience(Player player, Scp079PlayerScript scp, float exp, bool allow)
        {
            Player = player;
            Scp = scp;
            Experience = exp;
            IsAllowed = allow;
        }
    }
}
