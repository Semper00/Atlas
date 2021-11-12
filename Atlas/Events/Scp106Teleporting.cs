using Atlas.EventSystem;
using Atlas.Entities;

using UnityEngine;

namespace Atlas.Events
{
    /// <summary>
    /// Fires before SCP-106 uses it's teleport feature.
    /// </summary>
    public class Scp106Teleporting : BoolEvent
    {
        /// <summary>
        /// Gets the player who is controlling SCP-106.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets or sets the portal position SCP-106 is teleporting to.
        /// </summary>
        public Vector3 Position { get; set; }

        public Scp106Teleporting(Player scp, Vector3 pos, bool allow)
        {
            Player = scp;
            Position = pos;
            IsAllowed = allow;
        }
    }
}
