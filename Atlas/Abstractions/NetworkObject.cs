using UnityEngine;

namespace Atlas.Abstractions
{
    /// <summary>
    /// A class for network map objects (meaning they can be deleted and changed).
    /// </summary>
    public abstract class NetworkObject : MapObject
    {
        /// <summary>
        /// Gets the <see cref="UnityEngine.GameObject"/>.
        /// </summary>
        public abstract GameObject GameObject { get; }

        /// <summary>
        /// Gets or sets the scale.
        /// </summary>
        public abstract Vector3 Scale { get; set; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        public abstract Vector3 Position { get; set; }

        /// <summary>
        /// Gets or sets the rotation.
        /// </summary>
        public abstract Quaternion Rotation { get; set; }

        /// <summary>
        /// Deletes this object.
        /// </summary>
        public abstract void Delete();
    }
}
