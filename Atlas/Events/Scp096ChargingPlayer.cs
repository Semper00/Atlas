using Atlas.Entities;
using Atlas.EventSystem;

using PlayableScps;

namespace Atlas.Events
{
    /// <summary>
    /// Fires before SCP-096 charges at a player.
    /// </summary>
    public class Scp096ChargingPlayer : BoolEvent
    {
        /// <summary>
        /// Gets the SCP-096 instance.
        /// </summary>
        public Scp096 Scp { get; }

        /// <summary>
        /// Gets the player who is controlling SCP-096.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the player who SCP-096 is charging.
        /// </summary>
        public Player Victim { get; }

        /// <summary>
        /// Gets a value indicating whether the target is one of SCP-096's targets.
        /// </summary>
        public bool IsTarget { get; }

        /// <summary>
        /// Gets or sets the inflicted damage.
        /// </summary>
        public float Damage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether SCP-096's charge should be ended next frame.
        /// </summary>
        public bool EndCharge { get; set; }

        public Scp096ChargingPlayer(Player controller, Player victim, Scp096 scp, bool target, bool endCharge, bool allow, float damage)
        {
            Player = controller;
            Victim = victim;
            IsTarget = target;
            Damage = damage;
            EndCharge = endCharge;
            IsAllowed = allow;
        }
    }
}
