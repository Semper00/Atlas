using Atlas.Entities;
using Atlas.EventSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Fires before SCO-079 starts using a speaker.
    /// </summary>
    public class StartingSpeaker : BoolEvent
    {
        /// <summary>
        /// Gets the player who's controlling SCP-079.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the room that the speaker is located in.
        /// </summary>
        public Room Room { get; }

        /// <summary>
        /// Gets or sets the amount of auxiliary power required to use a speaker through SCP-079.
        /// </summary>
        public float AuxiliaryPowerCost { get; set; }

        public StartingSpeaker(Player player, Room room, float auxiliaryPowerCost, bool allow)
        {
            Player = player;
            Room = room;
            AuxiliaryPowerCost = auxiliaryPowerCost;
            IsAllowed = allow;
        }
    }
}
