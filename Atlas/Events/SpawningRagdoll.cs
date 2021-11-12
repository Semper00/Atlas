using UnityEngine;

using Atlas.Entities;
using Atlas.EventSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Fires before the server spawns a ragdoll.
    /// </summary>
    public class SpawningRagdoll : BoolEvent
    {
        private int playerId;

        /// <summary>
        /// Gets the player who killed the owner of the ragdoll.
        /// </summary>
        public Player Killer { get; }

        /// <summary>
        /// Gets the owner of the ragdoll (typically the player who died).
        /// </summary>
        public Player Owner { get; }

        /// <summary>
        /// Gets or sets the spawning position of the ragdoll.
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Gets or sets the ragdoll rotation.
        /// </summary>
        public Quaternion Rotation { get; set; }

        /// <summary>
        /// Gets or sets the adapted ragdoll velocity.
        /// </summary>
        public Vector3 Velocity { get; set; }

        /// <summary>
        /// Gets or sets the RoleType of the ragdoll owner.
        /// </summary>
        public RoleType RoleType { get; set; }

        /// <summary>
        /// Gets or sets the hit information on the ragdoll.
        /// </summary>
        public PlayerStats.HitInfo HitInfo { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the player can be revived by SCP-049.
        /// </summary>
        public bool IsRecallAllowed { get; set; }

        /// <summary>
        /// Gets or sets the ragdoll dissonance id.
        /// </summary>
        public string DissonanceId { get; set; }

        /// <summary>
        /// Gets or sets the ragdoll player nickname.
        /// </summary>
        public string PlayerNickname { get; set; }

        /// <summary>
        /// Gets or sets the ragdoll player id.
        /// </summary>
        public int PlayerId
        {
            get => playerId;
            set
            {
                if (PlayersList.Get(value) == null)
                    return;

                playerId = value;
            }
        }

        public SpawningRagdoll(
            Player killer,
            Player owner,
            Vector3 position,
            Quaternion rotation,
            Vector3 velocity,
            RoleType roleType,
            PlayerStats.HitInfo hinInformations,
            bool isRecallAllowed,
            string dissonanceId,
            string playerName,
            int playerId,
            bool isAllowed = true)
        {
            Killer = killer;
            Owner = owner;
            Position = position;
            Rotation = rotation;
            Velocity = velocity;
            RoleType = roleType;
            HitInfo = hinInformations;
            IsRecallAllowed = isRecallAllowed;
            DissonanceId = dissonanceId;
            PlayerNickname = playerName;
            PlayerId = playerId;
            IsAllowed = isAllowed;
        }
    }
}
