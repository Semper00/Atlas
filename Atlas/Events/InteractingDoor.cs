using Atlas.Entities;
using Atlas.EventSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Fires before a player interacts with a door.
    /// </summary>
    public class InteractingDoor : BoolEvent
    {
        /// <summary>
        /// Gets the player who is interacting.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the door that the player is interacting with.
        /// </summary>
        public Door Door { get; }

        public InteractingDoor(Player player, Door door, bool allow)
        {
            Player = player;
            Door = door;
            IsAllowed = allow;
        }
    }
}
