using Atlas.Enums;
using Atlas.Abstractions;

using UnityEngine;

namespace Atlas.Entities
{
    /// <summary>
    /// A wrapper for <see cref="PocketDimensionTeleport"/>.
    /// </summary>
    public class PocketDimensionExit : MapObject
    {
        internal PocketDimensionTeleport tp;

        internal bool exitChanged;

        public PocketDimensionExit(PocketDimensionTeleport tp)
            => this.tp = tp;

        /// <summary>
        /// Gets the pocket exit's <see cref="UnityEngine.GameObject"/>.
        /// </summary>
        public GameObject GameObject { get => tp.gameObject; }

        /// <summary>
        /// Gets the pocket exit's position.
        /// </summary>
        public Vector3 Position { get => tp.gameObject.transform.position; }

        /// <summary>
        /// Gets the pocket exit's rotation.
        /// </summary>
        public Quaternion Rotation { get => tp.gameObject.transform.rotation; }

        /// <summary>
        /// 
        /// </summary>
        public override uint NetId { get => tp.netId; }

        /// <summary>
        /// Gets or sets the exit's type.
        /// </summary>
        public ExitType ExitType
        {
            get => (ExitType)tp.GetTeleportType();
            set
            {
                tp.SetType((PocketDimensionTeleport.PDTeleportType)value);

                exitChanged = true;
            }
        }

        internal void InternalResetPocketDimensionExitType(PocketDimensionTeleport.PDTeleportType type)
            => tp.SetType(type);
    }
}
