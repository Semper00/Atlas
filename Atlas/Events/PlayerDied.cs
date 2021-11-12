using Atlas.Entities;
using Atlas.EventSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Fires AFTER a player dies.
    /// </summary>
    public class PlayerDied : Event
    {
        /// <summary>
        /// Gets the player that killed this player.
        /// </summary>
        public Player Killer { get; }

        /// <summary>
        /// Gets the player who died.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the hit information.
        /// </summary>
        public PlayerStats.HitInfo HitInfo { get; }

        public PlayerDied(Player killer, Player player, PlayerStats.HitInfo info)
        {
            Killer = killer;
            Player = player;
            HitInfo = info;
        }
    }
}
