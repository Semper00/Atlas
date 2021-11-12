using Atlas.Entities;
using Atlas.EventSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Fires before SCP-079 triggers a tesla gate.
    /// </summary>
    public class Scp079TriggeringTesla : BoolEvent
    {
        /// <summary>
        /// Gets the player who's controlling SCP-079.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the tesla gate that was triggered.
        /// </summary>
        public Tesla Tesla { get; }

        /// <summary>
        /// Gets or sets the amount of auxiliary power required to interact with a tesla gate through SCP-079.
        /// </summary>
        public float AuxiliaryPowerCost { get; set; }

        public Scp079TriggeringTesla(Player player, Tesla tesla, float cost, bool allow)
        {
            Player = player;
            Tesla = tesla;
            AuxiliaryPowerCost = cost;
            IsAllowed = allow;
        }
    }
}
