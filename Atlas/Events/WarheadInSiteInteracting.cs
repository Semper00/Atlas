using Atlas.Entities;
using Atlas.EventSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Fires when someone tries to interact with the in-site alpha warhead panel.
    /// </summary>
    public class WarheadInSiteInteracting : BoolEvent
    {
        /// <summary>
        /// Gets the player interacting.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the lever's current state.
        /// </summary>
        public bool State { get; }

        /// <summary>
        /// Gets or sets the lever's new state.
        /// </summary>
        public bool Target { get; set; }

        public WarheadInSiteInteracting(Player player, bool state, bool target, bool allow)
        {
            Player = player;
            State = state;
            Target = target;
        }
    }
}