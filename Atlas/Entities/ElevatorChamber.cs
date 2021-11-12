using UnityEngine;

using Atlas.Extensions;
using Atlas.Enums;

namespace Atlas.Entities
{
    /// <summary>
    /// Represents an elevator chamber.
    /// </summary>
    public struct ElevatorChamber
    {
        internal Lift.Elevator elevator;

        internal ElevatorChamber(Lift.Elevator elevator, int id, Elevator target, bool addToApi = false)
        {
            this.elevator = elevator;

            Id = id;
            Parent = target;

            if (addToApi)
                Map.elevatorObjects.Add(this);
        }

        /// <summary>
        /// Gets or sets the elevator's position.
        /// </summary>
        public Vector3 Position { get => elevator.pos; set => Destination = value; }

        /// <summary>
        /// Gets the elevator ID.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this chamber is open or not.
        /// </summary>
        public bool IsOpen { get => elevator.door.GetBool(Lift.IsOpen); set => elevator.door.SetBool(Lift.IsOpen, value); }

        /// <summary>
        /// Gets the elevator this chamber belongs to.
        /// </summary>
        public Elevator Parent { get; }

        /// <summary>
        /// Gets or sets the elevator's destination transoform.
        /// </summary>
        public Transform Target { get => elevator.target; set => elevator.target = value; }

        /// <summary>
        /// Gets or sets the elevator's destionation.
        /// </summary>
        public Vector3 Destination
        {
            get => elevator.GetPosition();
            set
            {
                Target.position = value;
                Target.gameObject.UnSpawn();
                Target.gameObject.Spawn();

                elevator.SetPosition();
            }
        }

        /// <summary>
        /// Gets the chamber type.
        /// </summary>
        public ElevatorChamberType Type { get => (ElevatorChamberType)Id; }

        /// <summary>
        /// Sets the status of colliders.
        /// </summary>
        public bool ColliderStatus { set => Parent?.lift.SetColliders(Id, value); }
    }
}
