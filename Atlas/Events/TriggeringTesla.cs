using Atlas.Entities;
using Atlas.EventSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Fires before a player triggers a tesla gate.
    /// </summary>
    public class TriggeringTesla : Event
    {
        /// <summary>
        /// Gets the player who triggered the tesla.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the player is in hurting range.
        /// </summary>
        public bool IsInHurtingRange { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the tesla is going to be activated.
        /// </summary>
        public bool IsTriggerable { get; set; }

        public TriggeringTesla(Player player, bool range, bool allow)
        {
            Player = player;
            IsInHurtingRange = range;
            IsTriggerable = allow;
        }
    }
}
