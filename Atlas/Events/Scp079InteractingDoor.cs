using Atlas.Entities;
using Atlas.EventSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Fires before SCP-079 interacts with a door.
    /// </summary>
    public class Scp079InteractingDoor : BoolEvent
    {
        /// <summary>
        /// Gets the player controlling SCP-079.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the <see cref="Scp079PlayerScript"/> instance.
        /// </summary>
        public Scp079PlayerScript Scp { get; }

        /// <summary>
        /// Gets or sets the amount of auxiliary power required to trigger a door through SCP-079.
        /// </summary>
        public float AuxiliaryPowerCost { get; set; }

        public Scp079InteractingDoor(Player player, float exp, bool allow)
        {
            Player = player;
            Scp = player.Hub.scp079PlayerScript;
            AuxiliaryPowerCost = exp;
            IsAllowed = allow;
        }
    }
}
