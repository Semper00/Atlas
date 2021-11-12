using Atlas.Entities;
using Atlas.EventSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Fires before SCP-096 pries open a gate.
    /// </summary>
    public class StartPryingGate : BoolEvent
    {
        /// <summary>
        /// Gets the player that is controlling SCP-096.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the <see cref="Entities.Door"/> to be pried open.
        /// </summary>
        public Door Door { get; }

        public StartPryingGate(Player player, Door door, bool allow)
        {
            Player = player;
            Door = door;
            IsAllowed = allow;
        }
    }
}
