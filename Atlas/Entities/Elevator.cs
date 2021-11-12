using System.Collections.Generic;
using System.Linq;

using Atlas.Enums;
using Atlas.Abstractions;
using Atlas.Extensions;
using Atlas.Entities.Pickups.Base;

using UnityEngine;

namespace Atlas.Entities
{
    /// <summary>
    /// A wrapper for <see cref="Lift"/>.
    /// </summary>
    public class Elevator : MapObject
    {
        internal Lift lift;

        internal List<ElevatorChamber> elevatorObjects;
        internal List<BasePickup> items;
        internal List<Ragdoll> ragdolls;
        internal List<Player> players;
        internal List<GameObject> buttons;

        internal Elevator(Lift lift, bool addToApi = false)
        {
            this.lift = lift;

            Type = GetType(Name);

            elevatorObjects = new List<ElevatorChamber>();
            items = new List<BasePickup>();
            ragdolls = new List<Ragdoll>();
            players = new List<Player>();
            buttons = new List<GameObject>();

            for (int i = 0; i < lift.elevators.Length; i++)
            {
                var chamber = new ElevatorChamber(lift.elevators[i], i, this, true);

                elevatorObjects.Add(chamber);

                foreach (var renderer in chamber.elevator.door.transform.parent.GetComponentsInChildren<MeshRenderer>())
                {
                    if (renderer.CompareTag("ElevatorButton"))
                    {
                        buttons.Add(renderer.gameObject);
                    }
                }
            }

            if (addToApi)
                Map.elevators.Add(this);
        }

        /// <inheritdoc/>
        public override uint NetId => lift.netId;

        /// <summary>
        /// Gets the elevator's name.
        /// </summary>
        public string Name { get => lift.elevatorName; }

        /// <summary>
        /// Gets the elevator's <see cref="UnityEngine.GameObject"/>.
        /// </summary>
        public GameObject GameObject { get => lift.gameObject; }

        /// <summary>
        /// Gets or sets the item's teleporting offset.
        /// </summary>
        public Vector3 ItemOffset { get => lift.itemOffset; set => lift.itemOffset = value; }

        /// <summary>
        /// Gets the elevator's type.
        /// </summary>
        public ElevatorType Type { get; }

        /// <summary>
        /// Gets or sets the elevator's status.
        /// </summary>
        public ElevatorStatus Status { get => (ElevatorStatus)lift.NetworkstatusID; set => lift.NetworkstatusID = (byte)value; }

        /// <summary>
        /// Gets the elevator chambers.
        /// </summary>
        public IReadOnlyList<ElevatorChamber> Chambers { get => elevatorObjects; }

        /// <summary>
        /// Gets the items.
        /// </summary>
        public IReadOnlyList<BasePickup> Items { get => items; }

        /// <summary>
        /// Gets a list of ragdolls.
        /// </summary>
        public IReadOnlyList<Ragdoll> Ragdolls { get => ragdolls; }

        /// <summary>
        /// Gets a list of players.
        /// </summary>
        public IReadOnlyList<Player> Players { get => players; }

        /// <summary>
        /// Gets a list of buttons.
        /// </summary>
        public IReadOnlyList<GameObject> Buttons { get => buttons; }

        /// <summary>
        /// Gets or sets a value indicating if this elevator can be locked or not.
        /// </summary>
        public bool IsLockable { get => lift.lockable; set => lift.lockable = value; }

        /// <summary>
        /// Gets or sets a value indicating if this elevator can be used by players or not.
        /// </summary>
        public bool IsLocked { get => lift.Network_locked; set => lift.Network_locked = value; }

        /// <summary>
        /// Gets a value indicating whether the elevator is operating or not.
        /// </summary>
        public bool IsOperating { get => !lift.operative; }

        /// <summary>
        /// Gets or sets a value indicating the maximal distance you can be from the elevator for it to teleport you.
        /// </summary>
        public float MaxDistance { get => lift.maxDistance; set => lift.maxDistance = value; }

        /// <summary>
        /// Gets or sets the elevator's moving speed (how long does it take to travel, in seconds).
        /// </summary>
        public float MovingSpeed { get => lift.movingSpeed; set => lift.movingSpeed = value; }

        /// <summary>
        /// Gets the object in range.
        /// </summary>
        /// <param name="objectPosition">The object to find.</param>
        /// <returns>The GameObject instance if it's in range, otherwise null.</returns>
        public GameObject GetObjectInRange(Vector3 objectPosition)
            => lift.InRange(objectPosition, out GameObject obj) ? obj : null;

        /// <summary>
        /// Locks this elevator.
        /// </summary>
        public void Lock()
            => lift.Lock();

        /// <summary>
        /// Moves players to the specified target.
        /// </summary>
        /// <param name="destination">The target to teleport to.</param>
        public void MovePlayers(ElevatorChamber destination)
            => lift.MovePlayers(destination.Target);

        /// <summary>
        /// Forces the music playback.
        /// </summary>
        public void PlayMusic()
            => lift.RpcPlayMusic();

        /// <summary>
        /// Makes this elevator move.
        /// </summary>
        public void Use()
            => lift.UseLift();

        /// <summary>
        /// Gets the chamber.
        /// </summary>
        /// <param name="type">The chamber to get.</param>
        /// <returns>The chamber.</returns>
        public ElevatorChamber GetChamber(ElevatorChamberType type)
            => elevatorObjects.First(x => x.Type == type);

        /// <summary>
        /// Gets the next elevator destination.
        /// </summary>
        /// <returns>The elevator destionation.</returns>
        public ElevatorChamber GetNextTarget()
        {
            foreach (var chamber in Chambers)
            {
                if (!chamber.IsOpen)
                    return chamber;
            }

            return default;
        }

        /// <summary>
        /// This method will update cached pickups and ragdolls inside this elevator. It gets called automatically, so no need to do it yourself.
        /// </summary>
        public void UpdateCachedItems()
        {
            items.Clear();
            ragdolls.Clear();

            foreach (var itemTransform in lift._cachedItemTransforms)
            {
                if (itemTransform.TryGetComponentInParent(out InventorySystem.Items.Pickups.ItemPickupBase pickup))
                {
                    BasePickup item = BasePickup.Get(pickup);

                    if (item == null)
                        continue;

                    items.Add(item);

                    continue;
                }

                if (itemTransform.TryGetComponentInParent(out global::Ragdoll baseRagdoll))
                {
                    Ragdoll ragdoll = Ragdoll.Get(baseRagdoll);

                    if (ragdoll == null)
                        continue;

                    ragdolls.Add(ragdoll);
                }
            }
        }

        #region API

        internal static ElevatorType GetType(string elevatorName)
        {
            Log.DebugFeature<Elevator>($"Elevator::FindType({elevatorName})");

            switch (elevatorName)
            {
                case "SCP-049":
                    return ElevatorType.Scp049;
                case "GateA":
                    return ElevatorType.GateA;
                case "GateB":
                    return ElevatorType.GateB;
                case "ElA":
                case "ElA2":
                    return ElevatorType.LczA;
                case "ElB":
                case "ElB2":
                    return ElevatorType.LczB;
                case "":
                    return ElevatorType.Nuke;

                default:
                    return ElevatorType.Unknown;
            }
        }

        #endregion
    }
}
