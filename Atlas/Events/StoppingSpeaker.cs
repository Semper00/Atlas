using Atlas.Entities;
using Atlas.EventSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Fires before SCP-079 stops using a speaker.
    /// </summary>
    public class StoppingSpeaker : BoolEvent
    {
        /// <summary>
        /// Gets the player who's controlling SCP-079.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the room that the speaker is located in.
        /// </summary>
        public Room Room { get; }

        public StoppingSpeaker(Player player, Room room, bool allow)
        {
            Player = player;
            Room = room;
            IsAllowed = allow;
        }
    }
}
