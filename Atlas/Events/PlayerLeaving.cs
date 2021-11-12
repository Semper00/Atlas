using Atlas.Entities;
using Atlas.EventSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Fires before a player leaves.
    /// </summary>
    public class PlayerLeaving : Event
    {
        /// <summary>
        /// Gets the leaving player.
        /// </summary>
        public Player Player { get; }

        public PlayerLeaving(Player player)
            => Player = player;
    }
}
