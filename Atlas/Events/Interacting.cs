using Atlas.Entities;
using Atlas.EventSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Fires before a player interacts.
    /// </summary>
    public class Interacting : Event
    {
        /// <summary>
        /// Gets the player who interacted.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to remove the Scp268 effect.
        /// </summary>
        public bool RemoveScp268Effect { get; set; }

        public Interacting(Player player, bool effect)
        {
            Player = player;
            RemoveScp268Effect = effect;
        }
    }
}
