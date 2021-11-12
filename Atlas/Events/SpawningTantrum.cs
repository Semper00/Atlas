using Atlas.Entities;
using Atlas.EventSystem;

using PlayableScps;
using UnityEngine;

namespace Atlas.Events
{
    /// <summary>
    /// Fires before SCP-173 spawns a tantrum.
    /// </summary>
    public class SpawningTantrum : BoolEvent
    {
        /// <summary>
        /// Gets the SCP-173 this tantrum belongs to.
        /// </summary>
        public Scp173 Scp { get; }

        /// <summary>
        /// Gets the player who's controlling SCP-173.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the tantrum's game object.
        /// </summary>
        public GameObject Tantrum { get; }

        /// <summary>
        /// Gets or sets the tantrum's position.
        /// </summary>
        public Vector3 Position { get; set; }

        public SpawningTantrum(Player player, Scp173 scp, GameObject tantrum, Vector3 pos, bool allow)
        {
            Scp = scp;
            Player = player;
            Tantrum = tantrum;
            Position = pos;
            IsAllowed = allow;

            InternalHandle();
        }

        internal void InternalHandle()
        {
            Map.tantrums.Add(Tantrum);
        }
    }
}
