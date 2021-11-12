using Scp914;
using Atlas.Entities;
using Atlas.EventSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Fires when a player activates SCP-914.
    /// </summary>
    public class ActivatingScp914 : BoolEvent
    {
        /// <summary>
        /// Gets the player that activated SCP-914.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the SCP-914 instance.
        /// </summary>
        public Scp914Controller Scp914 { get; }

        public ActivatingScp914(Player player, Scp914Controller scp914, bool allow)
        {
            Player = player;
            Scp914 = scp914;
            IsAllowed = allow;
        }
    }
}