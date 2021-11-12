using Atlas.Entities;
using Atlas.EventSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Fires when a player tries to insert a tablet into a workstation.
    /// </summary>
    public class ActivatingWorkstation : BoolEvent
    {
        /// <summary>
        /// Gets the player trying to activate a workstation.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the workstation.
        /// </summary>
        public Workstation Workstation { get; }

        public ActivatingWorkstation(Player player, Workstation workstation, bool allow)
        {
            Player = player;
            Workstation = workstation;
            IsAllowed = allow;
        }
    }
}
