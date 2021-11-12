using Atlas.Enums;
using Atlas.EventSystem;

using UnityEngine;

namespace Atlas.Events
{
    /// <summary>
    /// Fires before a blood decal spawns.
    /// </summary>
    public class PlacingBlood : BoolEvent
    {
        /// <summary>
        /// Gets or sets the decal's position.
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Gets or sets the decal's type.
        /// </summary>
        public BloodType Type { get; set; }

        public PlacingBlood(Vector3 pos, BloodType type, bool allow)
        {
            Position = pos;
            Type = type;
            IsAllowed = allow;
        }
    }
}
