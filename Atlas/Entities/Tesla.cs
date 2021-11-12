using UnityEngine;

using Mirror;

using Atlas.Abstractions;
using Atlas.Extensions;

namespace Atlas.Entities
{
    /// <summary>
    /// A wrapper for <see cref="TeslaGate"/>.
    /// </summary>
    public class Tesla : NetworkObject
    {
        internal TeslaGate gate;

        /// <summary>
        /// Initialzes a new instance of the <see cref="Tesla"/> class.
        /// </summary>
        /// <param name="gate">The original <see cref="TeslaGate"/>.</param>
        internal Tesla(TeslaGate gate, bool addToApi = false)
        {
            this.gate = gate;

            if (addToApi)
                Map.tesla.Add(this);
        }

        /// <summary>
        /// Gets the tesla's <see cref="UnityEngine.GameObject"/>.
        /// </summary>
        public override GameObject GameObject { get => gate.gameObject; }

        /// <summary>
        /// Gets a value indicating whether or not there is a burst in progress.
        /// </summary>
        public bool InProgress { get => gate.InProgress; }

        /// <summary>
        /// Gets the tesla's network ID.
        /// </summary>
        public override uint NetId { get => gate.netId; }

        /// <summary>
        /// Gets or sets the position of this tesla gate.
        /// </summary>
        public override Vector3 Position
        {
            get => gate.localPosition;
            set
            {
                NetworkServer.UnSpawn(GameObject);

                gate.enabled = false;
                gate.localPosition = value;
                gate.transform.position = value;

                NetworkServer.Spawn(GameObject);

                gate.enabled = true;
            }
        }

        /// <summary>
        /// Gets or sets the gate's scale.
        /// </summary>
        public override Vector3 Scale { get => gate.transform.localScale; set => gate.gameObject.Resize(value); }

        /// <summary>
        /// Gets or sets the rotation of this tesla gate.
        /// </summary>
        public override Quaternion Rotation
        {
            get => Quaternion.Euler(gate.localRotation);
            set
            {
                NetworkServer.UnSpawn(GameObject);

                gate.enabled = false;
                gate.localRotation = value.eulerAngles;
                gate.transform.rotation = value;

                NetworkServer.Spawn(GameObject);

                gate.enabled = true;
            }
        }

        /// <summary>
        /// Creates an instant burt.
        /// </summary>
        public void Burst()
            => gate.RpcInstantBurst();

        /// <summary>
        /// Deletes this gate.
        /// </summary>
        public override void Delete()
            => gate.gameObject.Delete();

        /// <summary>
        /// Tries to get a <see cref="Tesla"/> from a <see cref="TeslaGate"/>. This method will ALWAYS return an instance as it creates a new item in case it wasn't found.
        /// </summary>
        /// <param name="gate">The gate to find.</param>
        /// <returns>The tesla gate found, if any.</returns>
        public static Tesla Get(TeslaGate gate)
        {
            foreach (Tesla tesla in Map.tesla)
            {
                if (tesla.gate == gate || tesla.NetId == gate.netId)
                    return tesla;
            }

            return new Tesla(gate, true);
        }
    }
}
