using Atlas.Entities;
using Atlas.EventSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Fires when a player tries to eject a tablet from a workstation.
    /// </summary>
    public class DeactivatingWorkstation : BoolEvent
    {
        /// <summary>
        /// Gets the player trying to deactivate a workstation.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the workstation.
        /// </summary>
        public Workstation Workstation { get; }

        public DeactivatingWorkstation(Player player, Workstation workstation, bool allow)
        {
            Player = player;
            Workstation = workstation;
            IsAllowed = allow;
        }
    }
}
