using Atlas.EventSystem;
using Atlas.Entities;

namespace Atlas.Events
{
    /// <summary>
    /// Fires when SCP-914 tries to upgrade a player.
    /// </summary>
    public class UpgradingPlayer : BoolEvent
    {
        /// <summary>
        /// Gets the player that is being upgraded.
        /// </summary>
        public Player Player { get; }

        public UpgradingPlayer(Player player, bool allow)
        {
            Player = player;
            IsAllowed = allow;
        }
    }
}
