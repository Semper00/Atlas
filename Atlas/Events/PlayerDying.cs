using Atlas.Entities;
using Atlas.EventSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Fires before a player dies.
    /// </summary>
    public class PlayerDying : BoolEvent
    {
        /// <summary>
        /// Gets the player who is attacking this player.
        /// </summary>
        public Player Attacker { get; }

        /// <summary>
        /// Gets the player who is being attacked.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the hit information.
        /// </summary>
        public PlayerStats.HitInfo HitInfo { get; set; }

        public PlayerDying(Player attacker, Player player, PlayerStats.HitInfo info, bool allow)
        {
            Attacker = attacker;
            Player = player;
            HitInfo = info;
            IsAllowed = allow;
        }
    }
}
