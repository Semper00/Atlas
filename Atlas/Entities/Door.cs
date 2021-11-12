using System.Collections.Generic;
using System.Linq;

using Interactables.Interobjects;
using Interactables.Interobjects.DoorUtils;

using UnityEngine;

using Atlas.Enums;
using Atlas.Abstractions;
using Atlas.Extensions;

namespace Atlas.Entities
{
    /// <summary>
    /// A wrapper around the <see cref="DoorVariant"/> class for easier door management.
    /// </summary>
    public class Door : NetworkObject
    {
        internal DoorVariant door;
        internal bool prevState;

        /// <summary>
        /// Initializes a new instance of the <see cref="Door"/> class.
        /// </summary>
        /// <param name="door">The original door.</param>
        internal Door(DoorVariant door, bool addToApi = false)
        {
            this.door = door;

            NameTag = door.TryGetComponent<DoorNametagExtension>(out var name) ? name.GetName : null;

            Name = GetName(NameTag);
            Type = GetType(door);
            Room = Room.Get(door.gameObject);

            Blacklist = new HashSet<Player>();

            ScpBypass = door.RequiredPermissions.RequiredPermissions.HasFlagFast(KeycardPermissions.ScpOverride);

            if (addToApi)
                Map.doors.Add(this);
        }

        /// <summary>
        /// Gets the door's <see cref="UnityEngine.GameObject"/>.
        /// </summary>
        public override GameObject GameObject { get => door.gameObject; }

        /// <summary>
        /// Gets the room this door is tied to.
        /// </summary>
        public Room Room { get; }

        /// <summary>
        /// Gets the door's network ID. Same as the one for SCP-079's locked doors.
        /// </summary>
        public override uint NetId { get => door.netId; }

        /// <summary>
        /// Gets the door's name tag. Null if this door does not have one.
        /// </summary>
        public string NameTag { get; }

        /// <summary>
        /// Gets the base <see cref="DoorVariant"/>.
        /// </summary>
        public DoorVariant Base { get => door; }

        /// <summary>
        /// Gets or sets the door's position.
        /// </summary>
        public override Vector3 Position { get => GameObject.transform.position; set => GameObject.Teleport(value); }

        /// <inheritdoc/>
        public override Vector3 Scale { get => GameObject.transform.localScale; set => GameObject.Resize(value); }

        /// <summary>
        /// Gets or sets the door's rotation.
        /// </summary>
        public override Quaternion Rotation { get => GameObject.transform.rotation; set => GameObject.Rotate(value); }

        /// <summary>
        /// Gets or sets active door locks.
        /// </summary>
        public DoorLockReason ActiveLocks { get => (DoorLockReason) door.NetworkActiveLocks; set => door.NetworkActiveLocks = (ushort) value; }

        /// <summary>
        /// Gets the door type.
        /// </summary>
        public DoorType Type { get; }

        /// <summary>
        /// Gets the <see cref="DoorName"/>.
        /// </summary>
        public DoorName Name { get; }

        /// <summary>
        /// Gets a <see cref="HashSet{T}"/> of players that cannot use this door.
        /// </summary>
        public HashSet<Player> Blacklist { get; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the door is open.
        /// </summary>
        public bool IsOpen
        {
            get => door.NetworkTargetState;
            set
            {
                prevState = door.NetworkTargetState;

                door.NetworkTargetState = value;
            }
        }

        /// <summary>
        /// Gets the door's previous state.
        /// </summary>
        public bool PreviousState { get => prevState; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to destroy this door upon interaction.
        /// </summary>
        public bool DestroyOnInteraction { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether SCPs can open this door or not.
        /// </summary>
        public bool ScpBypass { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if this door is destroyed or not.
        /// </summary>
        public bool IsDestroyed { get => door is BreakableDoor breakable && breakable.Network_destroyed; set => (door as BreakableDoor).Network_destroyed = true; }

        /// <summary>
        /// Changes the lock value.
        /// </summary>
        /// <param name="reason">The reason.</param>
        /// <param name="value">The new value.</param>
        public void ChangeLock(DoorLockReason reason, bool value) => door.ServerChangeLock(reason, value);

        /// <summary>
        /// Locks this door.
        /// </summary>
        public void Lock() 
            => ChangeLock(DoorLockReason.AdminCommand, true);

        /// <summary>
        /// Unlocks this door.
        /// </summary>
        public void Unlock() 
            => ChangeLock(DoorLockReason.AdminCommand, false);

        /// <summary>
        /// Destroys this door (if it can be destroyed).
        /// </summary>
        public void Destroy()
        {
            if (door is BreakableDoor b)
                b.Network_destroyed = true;
        }

        /// <summary>
        /// Sets the door's state to it's previous state.
        /// </summary>
        public void ToPreviousState()
            => IsOpen = PreviousState;

        /// <summary>
        /// Deletes this door.
        /// </summary>
        public override void Delete()
            => GameObject.Delete();

        /// <summary>
        /// Gets the door as the specified door type.
        /// </summary>
        /// <typeparam name="T">The door type.</typeparam>
        /// <returns>The door as the specified type.</returns>
        public T As<T>() where T : DoorVariant
        {
            if (Base is T tDoor)
                return tDoor;

            return default;
        }

        /// <summary>
        /// Tries to get a <see cref="Door"/> from a <see cref="DoorVariant"/>.
        /// </summary>
        /// <param name="door">The <see cref="DoorVariant"/> to search by.</param>
        /// <returns>The found door, null if none.</returns>
        public static Door Get(DoorVariant door)
        {
            foreach (Door d in Map.Doors)
            {
                if (d.NetId == door.netId)
                    return d;
            }

            return null;
        }

        /// <summary>
        /// Tries to get a <see cref="Door"/> from a <see cref="DoorName"/>.
        /// </summary>
        /// <param name="type">The <see cref="DoorName"/> to search by.</param>
        /// <returns>The found door, null if none.</returns>
        public static Door Get(DoorName type)
        {
            foreach (Door door in Map.Doors)
                if (door.Name == type)
                    return door;

            return null;
        }

        /// <summary>
        /// Tries to get a <see cref="Door"/> from a <see cref="string"/>.
        /// </summary>
        /// <param name="name">The <see cref="string"/> to search by.</param>
        /// <returns>The found door, null if none.</returns>
        public static Door Get(string name)
        {
            foreach (Door door in Map.Doors)
                if (door.NameTag != null && door.NameTag == name)
                    return door;

            return Get(GetName(name));
        }

        /// <summary>
        /// Tries to find a <see cref="Door"/> by it's network ID.
        /// </summary>
        /// <param name="netId">The network ID to search by.</param>
        /// <returns>The <see cref="Door"/> found, if any.</returns>
        public static Door Get(uint netId)
        {
            foreach (Door door in Map.doors)
                if (door.NetId == netId)
                    return door;

            return null;
        }

        /// <summary>
        /// Attempts to find every door object in a <see cref="IEnumerable{T}"/> of <see cref="uint"/>.
        /// </summary>
        /// <param name="netIdList">The list to fill.</param>
        /// <returns>The list of doors.</returns>
        public static List<Door> Get(IEnumerable<uint> netIdList)
        {
            List<Door> doors = new List<Door>(netIdList.Count());

            foreach (uint netId in netIdList)
            {
                var door = Get(netId);

                if (door != null)
                    doors.Add(door);
            }

            return doors;
        }

        /// <summary>
        /// Spawns a door.
        /// </summary>
        /// <param name="type">The door type to spawn.</param>
        /// <param name="pos">The position to spawn the door at.</param>
        /// <param name="scale">The size of the door.</param>
        /// <param name="rot">The rotation to spawn the door with.</param>
        /// <returns>The spawned door, if succesfull, otherwise null.</returns>
        public static Door Spawn(DoorType type, Vector3 pos, Vector3 scale, Quaternion rot)
        {
            Prefab prefab = null;

            switch (type)
            {
                case DoorType.LczDoor:
                    prefab = Prefab.GetPrefab(PrefabType.LCZ_BreakableDoor);
                    break;
                case DoorType.HczDoor:
                    prefab = Prefab.GetPrefab(PrefabType.HCZ_BreakableDoor);
                    break;
                case DoorType.EzDoor:
                    prefab = Prefab.GetPrefab(PrefabType.EZ_BreakableDoor);
                    break;
                default:
                    throw new System.InvalidOperationException("This should not happen.");
            }

            if (prefab == null)
                return null;

            return new Door(prefab.Spawn(pos, scale, rot, true).GetComponent<DoorVariant>(), true);
        }

        /// <summary>
        /// Clones this door.
        /// </summary>
        /// <returns>The cloned door if succesfull, otherwise null.</returns>
        public Door Clone()
            => Spawn(Type, Position, Scale, Rotation);

        /// <summary>
        /// Gets the door object for SCP-173's gate.
        /// </summary>
        public static Door Scp173Gate => Get(DoorName.Scp173Gate);

        /// <summary>
        /// Gets the SCP-914's intake door.
        /// </summary>
        public static Door Scp914Intake => Map.Scp914.IntakeDoor;

        /// <summary>
        /// Gets the SCP-914's output door.
        /// </summary>
        public static Door Scp914Output => Map.Scp914.OutputDoor;

        #region API

        internal static DoorName GetName(string doorName)
        {
            Log.DebugFeature<Door>($"Door::GetName({doorName})");

            switch (doorName)
            {
                case "Prison BreakableDoor":
                    return DoorName.PrisonDoor;
                case "CHECKPOINT_LCZ_A":
                    return DoorName.CheckpointLczA;
                case "CHECKPOINT_EZ_HCZ":
                    return DoorName.CheckpointEntrance;
                case "CHECKPOINT_LCZ_B":
                    return DoorName.CheckpointLczB;
                case "106_PRIMARY":
                    return DoorName.Scp106Primary;
                case "106_SECONDARY":
                    return DoorName.Scp106Secondary;
                case "106_BOTTOM":
                    return DoorName.Scp106Bottom;
                case "ESCAPE_PRIMARY":
                    return DoorName.EscapePrimary;
                case "ESCAPE_SECONDARY":
                    return DoorName.EscapeSecondary;
                case "INTERCOM":
                    return DoorName.Intercom;
                case "NUKE_ARMORY":
                    return DoorName.NukeArmory;
                case "LCZ_ARMORY":
                    return DoorName.LczArmory;
                case "012":
                    return DoorName.Scp012;
                case "SURFACE_NUKE":
                    return DoorName.NukeSurface;
                case "HID":
                    return DoorName.HID;
                case "HCZ_ARMORY":
                    return DoorName.HczArmory;
                case "096":
                    return DoorName.Scp096;
                case "049_ARMORY":
                    return DoorName.Scp049Armory;
                case "914":
                    return DoorName.Scp914;
                case "GATE_A":
                    return DoorName.GateA;
                case "079_FIRST":
                    return DoorName.Scp079First;
                case "GATE_B":
                    return DoorName.GateB;
                case "079_SECOND":
                    return DoorName.Scp079Second;
                case "012_LOCKER":
                    return DoorName.Scp012Locker;
                case "SERVERS_BOTTOM":
                    return DoorName.ServersBottom;
                case "173_CONNECTOR":
                    return DoorName.Scp173Connector;
                case "LCZ_WC":
                    return DoorName.LczWc;
                case "HID_RIGHT":
                    return DoorName.HIDRight;
                case "012_BOTTOM":
                    return DoorName.Scp012Bottom;
                case "HID_LEFT":
                    return DoorName.HIDLeft;
                case "173_ARMORY":
                    return DoorName.Scp173Armory;
                case "173_GATE":
                    return DoorName.Scp173Gate;
                case "GR18":
                    return DoorName.GR18;
                case "SURFACE_GATE":
                    return DoorName.SurfaceGate;

                default:
                    doorName = doorName.GetBefore(' ');

                    switch (doorName)
                    {
                        case "LCZ":
                            return DoorName.LightContainmentDoor;
                        case "HCZ":
                            return DoorName.HeavyContainmentDoor;
                        case "EZ":
                            return DoorName.EntranceDoor;
                        default:
                            return DoorName.UnknownDoor;
                    }
            }
        }

        internal static DoorType GetType(DoorVariant door)
        {
            Log.DebugFeature<Door>($"Door::GetType({door})");

            if (door is BasicDoor)
            {
                if (door.name.Contains("EZ"))
                    return DoorType.EzDoor;

                if (door.name.Contains("HCZ"))
                    return DoorType.HczDoor;

                if (door.name.Contains("LCZ"))
                    return DoorType.LczDoor;
            }

            return DoorType.EzDoor;
        }

        #endregion
    }
}