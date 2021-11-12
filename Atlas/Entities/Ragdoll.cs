using System.Linq;

using UnityEngine;
using Mirror;

using Object = UnityEngine.Object;

using Atlas.Extensions;
using Atlas.Abstractions;

namespace Atlas.Entities
{
    /// <summary>
    /// A set of tools to handle the ragdolls more easily.
    /// </summary>
    public class Ragdoll : NetworkObject
    {
        internal global::Ragdoll ragdoll;

        internal Ragdoll(global::Ragdoll ragdoll, bool addToApi = false)
        {
            this.ragdoll = ragdoll;

            if (addToApi)
                Map.ragdolls.Add(this);
        }

        /// <inheritdoc/>
        public override void Delete()
        {
            NetworkServer.Destroy(GameObject);

            Map.ragdolls.Remove(this);
        }

        /// <inheritdoc/>
        public override GameObject GameObject { get => ragdoll.gameObject; }

        /// <inheritdoc/>
        public override uint NetId { get => ragdoll.netId; }

        /// <inheritdoc/>
        public override Vector3 Position { get => ragdoll.transform.position; set => ragdoll.gameObject.Teleport(value); }

        /// <inheritdoc/>
        public override Vector3 Scale { get => ragdoll.transform.localScale; set => ragdoll.gameObject.Resize(value); }

        /// <inheritdoc/>
        public override Quaternion Rotation { get => ragdoll.transform.rotation; set => ragdoll.gameObject.Rotate(value); }

        /// <summary>
        /// Gets the class color.
        /// </summary>
        public Color ClassColor { get => ragdoll.Networkowner.ClassColor; }

        /// <summary>
        /// Gets the ragdoll's owner.
        /// </summary>
        public Player Owner { get => PlayersList.Get(ragdoll.Networkowner.PlayerId); }

        /// <summary>
        /// Gets or sets the ragdoll's death cause.
        /// </summary>
        public PlayerStats.HitInfo DeathCause
        {
            get => ragdoll.Networkowner.DeathCause;
            set
            {
                var info = ragdoll.Networkowner;

                info.DeathCause = value;

                ragdoll.Networkowner = info;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this ragdoll can be recalled by SCP-049 or not.
        /// </summary>
        public bool AllowRecall { get => ragdoll.NetworkallowRecall; set => ragdoll.NetworkallowRecall = value; }

        /// <summary>
        /// Gets or sets the time that passed since this ragdoll spawned.
        /// </summary>
        public float CurrentTime { get => ragdoll.CurrentTime; set => ragdoll.CurrentTime = value; }

        /// <summary>
        /// Gets or sets a number that sets the maximum time since spawning for SCP-049 to be able to revive this ragdoll.
        /// </summary>
        public int MaxRagdollTime { get => ragdoll.MaxRagdollTime; set => ragdoll.MaxRagdollTime = value; }

        /// <summary>
        /// Spawns a ragdoll based on the provided ragdoll info.
        /// </summary>
        /// <param name="ragdollInfo">The parameters to spawn with.</param>
        /// <returns>The spawned ragdoll.</returns>
        public static Ragdoll Spawn(RagdollInfo ragdollInfo)
        {
            var role = CharacterClassManager._staticClasses.SafeGet(ragdollInfo.Role);

            if (role != null)
            {
                global::Ragdoll ragdoll = Object.Instantiate(role.model_ragdoll, ragdollInfo.Position + role.ragdoll_offset.position, 
                    Quaternion.Euler(ragdollInfo.Rotation.eulerAngles + role.ragdoll_offset.rotation)).GetComponent<global::Ragdoll>();

                ragdoll.NetworkallowRecall = ragdollInfo.AllowRecall;
                ragdoll.Networkowner = ragdollInfo.ToBase();
                ragdoll.NetworkPlayerVelo = ragdollInfo.Velocity;

                ragdoll.gameObject.Spawn();

                return new Ragdoll(ragdoll, true);
            }

            return null;
        }

        /// <summary>
        /// Tries to get a <see cref="Ragdoll"/> from a <see cref="global::Ragdoll"/>. This method will ALWAYS return an instance as it creates a new item in case it wasn't found.
        /// </summary>
        /// <param name="ragdoll">The ragdoll to find.</param>
        /// <returns>The ragdoll found.</returns>
        public static Ragdoll Get(global::Ragdoll ragdoll)
        {
            foreach (Ragdoll rag in Map.ragdolls)
            {
                if (rag.ragdoll == ragdoll || rag.NetId == ragdoll.netId)
                    return rag;
            }

            return new Ragdoll(ragdoll, true);
        }
    }
}
