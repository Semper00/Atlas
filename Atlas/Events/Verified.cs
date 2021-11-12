using Atlas.EventSystem;
using Atlas.Entities;

namespace Atlas.Events
{
    /// <summary>
    /// Fires after a player gets verified by the server.
    /// </summary>
    public class Verified : Event
    { 
        /// <summary>
        /// Gets the verified player.
        /// </summary>
        public Player Player { get; }

        public Verified(Player player) 
            => Player = player;
    }
}
