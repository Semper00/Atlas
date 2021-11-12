using UnityEngine;

using Atlas.Extensions;
using Atlas.Abstractions;

using InventorySystem.Items.Firearms.Attachments;

namespace Atlas.Entities
{
    /// <summary>
    /// A wrapper for <see cref="WorkStation"/>.
    /// </summary>
    public class Workstation : NetworkObject
    {
        internal WorkstationController station;

        /// <summary>
        /// Initialzes a new instance of the <see cref="Workstation"/> class.
        /// </summary>
        /// <param name="station"></param>
        public Workstation(WorkstationController station, bool addToApi = false)
        {
            this.station = station;

            if (addToApi)
                Map.stations.Add(this);
        }

        /// <summary>
        /// Gets the workstation's <see cref="UnityEngine.GameObject"/>.
        /// </summary>
        public override GameObject GameObject { get => station.gameObject; }

        /// <summary>
        /// Gets the station's network ID.
        /// </summary>
        public override uint NetId { get => station.netId; }

        /// <summary>
        /// Gets or sets the workstation's position.
        /// </summary>
        public override Vector3 Position { get => station.transform.position; set => GameObject.Teleport(value); }

        /// <summary>
        /// Gets or sets the workstation's rotation.
        /// </summary>
        public override Quaternion Rotation { get => station.transform.rotation; set => GameObject.Rotate(value); }

        /// <summary>
        /// Gets or sets the workstation's scale.
        /// </summary>
        public override Vector3 Scale { get => station.transform.localScale; set => GameObject.Resize(value); }

        /// <summary>
        /// Gets the currently active workstation user. Will return null if there isnt one.
        /// </summary>
        public Player User { get => PlayersList.Get(station._knownUser); }

        /// <summary>
        /// Deletes this workstation.
        /// </summary>
        public override void Delete()
            => station.gameObject.Delete();

        /// <summary>
        /// Tries to get a <see cref="Workstation"/> from <see cref="WorkStation"/>. This method will ALWAYS return an instance as it creates a new item in case it wasn't found.
        /// </summary>
        /// <param name="station">The station to find.</param>
        /// <returns>The station found.</returns>
        public static Workstation Get(WorkstationController station)
        {
            foreach (Workstation workstation in Map.stations)
            {
                if (workstation.NetId == station.netId || workstation.station == station)
                    return workstation;
            }

            return new Workstation(station, true);
        }
    }
}