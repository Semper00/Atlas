using Atlas.Extensions;

using UnityEngine;

namespace Atlas.Entities
{
    /// <summary>
    /// Used to provide information when spawning a ragdoll.
    /// </summary>
    public struct RagdollInfo
    {
        /// <summary>
        /// Gets the ragdoll's role.
        /// </summary>
        public RoleType Role;

        /// <summary>
        /// Gets the ragdoll's death cause.
        /// </summary>
        public PlayerStats.HitInfo DeathCause;

        /// <summary>
        /// Gets the ragdoll's role name.
        /// </summary>
        public string RoleName;

        /// <summary>
        /// Gets the ragdoll's owner name.
        /// </summary>
        public string Name;

        /// <summary>
        /// Gets the ragdoll's owner ID.
        /// </summary>
        public int OwnerId;

        /// <summary>
        /// Gets a value indicating whether or not to allow SCP-049 to recall this ragdoll.
        /// </summary>
        public bool AllowRecall;

        /// <summary>
        /// Gets the position to spawn the ragdoll at.
        /// </summary>
        public Vector3 Position;

        /// <summary>
        /// Gets the size of the ragdoll.
        /// </summary>
        public Vector3 Scale;

        /// <summary>
        /// Gets the ragdoll's velocity.
        /// </summary>
        public Vector3 Velocity;

        /// <summary>
        /// Gets the ragdoll's model rotation.
        /// </summary>
        public Quaternion Rotation;

        /// <summary>
        /// Returns an instance of <see cref="global::Ragdoll.Info"/> with the specified parameters.
        /// </summary>
        /// <returns>The instance.</returns>
        public global::Ragdoll.Info ToBase()
        {
            return new global::Ragdoll.Info
            {
                ClassColor = Role.GetColor(),
                DeathCause = DeathCause,
                FullName = RoleName,
                Nick = Name,
                ownerHLAPI_id = null,
                PlayerId = OwnerId
            };
        }
    }
}
