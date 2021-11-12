using Atlas.Entities;

using Atlas.EventSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Fires before a player fails to escape the pocket dimension.
    /// </summary>
    public class FailingPocketEscape : BoolEvent
    {
        /// <summary>
        /// Gets the player who is failing to escape.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the pocket dimension exit the player has walked into.
        /// </summary>
        public PocketDimensionExit Exit { get; }

        public FailingPocketEscape(Player player, PocketDimensionExit ex, bool allow)
        {
            Player = player;
            Exit = ex;
            IsAllowed = allow;
        }
    }
}