using UnityEngine;

using Atlas.Entities;
using Atlas.EventSystem;

namespace Atlas.Events
{
    public class Shot : BoolEvent
    {
        /// <summary>
        /// Gets the player who shot.
        /// </summary>
        public Player Shooter { get; }

        /// <summary>
        /// Gets the target of the shot. Can be null!.
        /// </summary>
        public Player Target { get; }

        /// <summary>
        /// Gets the hitbox type of the shot. Can be null!.
        /// </summary>
        public HitboxIdentity Hitbox { get; }

        /// <summary>
        /// Gets the shot distance.
        /// </summary>
        public float Distance { get; }

        /// <summary>
        /// Gets or sets the inflicted damage.
        /// </summary>
        public float Damage { get; set; }

        public Shot(Player shooter, RaycastHit hit, IDestructible destructible, float damage, bool allow)
        {
            Shooter = shooter;
            Damage = damage;
            Distance = hit.distance;

            if (destructible is HitboxIdentity id)
            {
                Hitbox = id;
                Target = PlayersList.Get(id.TargetHub);
            }

            IsAllowed = true;
        }
    }
}
