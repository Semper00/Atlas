using Atlas.EventSystem;
using Atlas.Entities;

namespace Atlas.Events
{
    /// <summary>
    /// Fires when someone interacts with the warhead outsite panel lever.
    /// </summary>
    public class WarheadOutsiteInteracting : BoolEvent
    {
        /// <summary>
        /// Gets the player that interacted.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the current lever state.
        /// </summary>
        public bool State { get; }

        /// <summary>
        /// Gets or sets the new lever state.
        /// </summary>
        public bool Target { get; set; }

        public WarheadOutsiteInteracting(Player player, bool state, bool target, bool allow)
        {
            Player = player;
            State = state;
            Target = target;
            IsAllowed = allow;
        }
    }
}
