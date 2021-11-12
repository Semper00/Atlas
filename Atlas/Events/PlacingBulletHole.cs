using Atlas.Entities;
using Atlas.EventSystem;

using UnityEngine;

namespace Atlas.Events
{
    /// <summary>
    /// Fires before a bullet hole spawns.
    /// </summary>
    public class PlacingBulletHole : BoolEvent
    {
        /// <summary>
        /// Gets the decal owner.
        /// </summary>
        public Player Owner { get; }

        /// <summary>
        /// Gets or sets the decal position.
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Gets or sets the decal rotation.
        /// </summary>
        public Quaternion Rotation { get; set; }

        public PlacingBulletHole(Player owner, Vector3 pos, Quaternion rot, bool allow)
        {
            Owner = owner;
            Position = pos;
            Rotation = rot;
            IsAllowed = allow;
        }
    }
}
