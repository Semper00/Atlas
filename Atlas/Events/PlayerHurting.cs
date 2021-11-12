using Atlas.Entities;
using Atlas.EventSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Fires before a player takes damage.
    /// </summary>
    public class PlayerHurting : BoolEvent
    {
        internal PlayerStats.HitInfo hit;

        /// <summary>
        /// Gets the attacker player.
        /// </summary>
        public Player Attacker { get; }

        /// <summary>
        /// Gets the target player, who is going to be hurt.
        /// </summary>
        public Player Target { get; }

        /// <summary>
        /// Gets the hit information.
        /// </summary>
        public PlayerStats.HitInfo HitInfo { get => hit; }

        /// <summary>
        /// Gets the time at which the player was hurt.
        /// </summary>
        public int Time { get => hit.Time; }

        /// <summary>
        /// Gets the damage type.
        /// </summary>
        public DamageTypes.DamageType DamageType { get => hit.Tool; }

        /// <summary>
        /// Gets or sets the amount of inflicted damage.
        /// </summary>
        public float Amount { get => hit.Amount; set => hit.Amount = value; }

        public PlayerHurting(Player attacker, Player target, PlayerStats.HitInfo hit, bool allow)
        {
            this.hit = hit;

            Attacker = attacker;
            Target = target;
            IsAllowed = allow;
        }
    }
}
