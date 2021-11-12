using Atlas.Abstractions;
using Atlas.Enums;

using UnityEngine;

using MapGeneration;

using NorthwoodLib.Pools;

using Interactables.Interobjects.DoorUtils;

using System.Collections.Generic;
using System.Linq;

using MEC;

namespace Atlas.Entities
{
    /// <summary>
    /// The in-game room.
    /// </summary>
    public class Room : MapObject
    {
        internal RoomIdentifier id;

        internal List<Player> players;
        internal List<Camera> cams;
        internal List<Door> doors;

        internal static readonly Dictionary<int, Room> rooms = new Dictionary<int, Room>();

        /// <summary>
        /// Gets all rooms on the map.
        /// </summary>
        public static IReadOnlyDictionary<int, Room> AllRooms { get => rooms; }

        /// <summary>
        /// Gets the default color for rooms.
        /// </summary>
        public static Color DefaultLightColor { get => FlickerableLightController.DefaultWarheadColor; }

        /// <summary>
        /// Gets the door's network ID.
        /// </summary>
        public override uint NetId { get; }

        /// <summary>
        /// Gets the room's unique ID.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets the room's name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the room's type.
        /// </summary>
        public RoomType Type { get; }

        /// <summary>
        /// Gets the room's shape.
        /// </summary>
        public Enums.RoomShape Shape { get; }

        /// <summary>
        /// Gets the zone the room is located in.
        /// </summary>
        public ZoneType Zone { get; }

        /// <summary>
        /// Gets the room's position.
        /// </summary>
        public Vector3 Position { get; }

        /// <summary>
        /// Gets the position for safe teleportation.
        /// </summary>
        public Vector3 SafePosition
        {
            get
            {
                var pos = Position;

                pos.y += 1.35f;

                return pos;
            }
        }

        /// <summary>
        /// Gets the room's <see cref="UnityEngine.Transform"/>.
        /// </summary>
        public Transform Transform { get; }

        /// <summary>
        /// Gets the room's <see cref="UnityEngine.GameObject"/>.
        /// </summary>
        public GameObject GameObject { get; }

        /// <summary>
        /// Gets the room's <see cref="FlickerableLightController"/>.
        /// </summary>
        public FlickerableLightController Light { get; }

        /// <summary>
        /// Gets the room's <see cref="RoomIdentifier"/>.
        /// </summary>
        public RoomIdentifier Identifier { get => id; }

        /// <summary>
        /// Gets or sets the room's lights intensity.
        /// </summary>
        public float LightIntensity { get => Light.Network_lightIntensityMultiplier; set => Light.Network_lightIntensityMultiplier = value; }

        /// <summary>
        /// Gets or sets a value indicating whether the lights are off or not.
        /// </summary>
        public bool AreLightsOff
        {
            get => Light && Light.IsEnabled();
            set
            {
                if (!value)
                {
                    Light.FlickerDuration = 0f;

                    return;
                }

                Light.ServerFlickerLights(float.MaxValue);
            }
        }

        /// <summary>
        /// Gets or sets the room's lights color.
        /// </summary>
        public Color LightColor
        {
            get => Light.Network_warheadLightColor;
            set
            {
                if (value == DefaultLightColor)
                {
                    Light.Network_warheadLightColor = value;
                    Light.Network_warheadLightOverride = false;

                    return;
                }

                Light.Network_warheadLightColor = value;
                Light.Network_warheadLightOverride = true;
            }
        }

        /// <summary>
        /// Gets the list of players in this room.
        /// </summary>
        public IReadOnlyList<Player> Players
        {
            get
            {
                players.Clear();

                foreach (Player player in PlayersList.players.Values)
                {
                    if (player.Room.Id == Id)
                        players.Add(player);
                }

                return players;
            }
        }

        /// <summary>
        /// Gets the list of doors in this room.
        /// </summary>
        public IReadOnlyList<Door> Doors { get => doors; }

        /// <summary>
        /// Gets the list of cameras in this room.
        /// </summary>
        public IReadOnlyList<Camera> Cameras { get => cams; }

        /// <summary>
        /// Gets the list of lights in this room.
        /// </summary>
        public IReadOnlyList<FlickerableLight> Lights { get => Light.LightsInRoom; }

        internal Room(RoomIdentifier id, bool addToApi = false)
        {
            this.id = id;

            Type = (RoomType)id.Name;
            Name = id.name;
            Shape = (Enums.RoomShape)id.Shape;
            Zone = (ZoneType)id.Zone;
            Position = id.transform.position;
            Transform = id.transform;
            GameObject = id.gameObject;
            Light = id.GetComponentInChildren<FlickerableLightController>();
            NetId = Light?.netId ?? 0;
            Id = id.UniqueId;

            players = ListPool<Player>.Shared.Rent();
            cams = ListPool<Camera>.Shared.Rent();
            doors = ListPool<Door>.Shared.Rent();

            FindCamerasAndDoors();

            Log.DebugFeature<Room>($"A new Room has been created ({NetId} {Type} {Name} {Shape} {Zone})");

            if (addToApi)
            {
                if (!rooms.ContainsKey(Id))
                    rooms.Add(Id, this);
            }
        }

        ~Room()
        {
            ListPool<Player>.Shared.Return(players);
            ListPool<Camera>.Shared.Return(cams);
            ListPool<Door>.Shared.Return(doors);
        }

        /// <summary>
        /// Turns off the lights for the specified time.
        /// </summary>
        /// <param name="duration">The duration to keep the lights off for.</param>
        public void FlickerLights(float duration)
        {
            Light.ServerFlickerLights(duration);
        }

        /// <summary>
        /// Turns off the lights and locks the doors for the specified time.
        /// </summary>
        /// <param name="duration">The duration of the blackout.</param>
        public void Blackout(float duration)
        {
            FlickerLights(duration);
            Lockdown(duration);
        }

        /// <summary>
        /// Locks the doors and unlocks them after the specified time.
        /// </summary>
        /// <param name="duration">The time to wait.</param>
        public void Lockdown(float duration)
        {
            LockAndCloseDoors();

            if (duration <= 0f)
                return;

            Timing.CallDelayed(duration, () => { doors.ForEach(x => x.Unlock()); });
        }

        /// <summary>
        /// Locks the doors and revers them to their previous state after the specified time.
        /// </summary>
        /// <param name="duration">The time to wait.</param>
        public void LockdownAndRevert(float duration)
        {
            LockAndCloseDoors();

            if (duration <= 0f)
                return;

            Timing.CallDelayed(duration, () =>
            {
                doors.ForEach(x =>
                {
                    x.Unlock();
                    x.ToPreviousState();
                });
            });
        }

        /// <summary>
        /// Locks and closes all doors in this room.
        /// </summary>
        public void LockAndCloseDoors()
        {
            doors.ForEach(x =>
            {
                x.IsOpen = false;
                x.Lock();
            });
        }

        /// <summary>
        /// Locks all doors in this room.
        /// </summary>
        public void LockDoors()
            => doors.ForEach(x => x.Lock());

        /// <summary>
        /// Unlocks all doors in this room.
        /// </summary>
        public void UnlockDoors()
            => doors.ForEach(x => x.Unlock());

        /// <summary>
        /// Resets the room's light color.
        /// </summary>
        public void ResetLightColor()
        {
            Light.Network_warheadLightColor = DefaultLightColor;
        }

        internal void FindCamerasAndDoors()
        {
            foreach (var door in id.GetComponentsInChildren<DoorVariant>())
            {
                var apiDoor = Door.Get(door);

                if (apiDoor != null)
                    doors.Add(apiDoor);
            }

            foreach (var cam in Scp079PlayerScript.allCameras)
            {
                var camera = Camera.Get(cam);

                if (camera != null)
                {
                    if (camera.Room != null && camera.Room.GameObject == GameObject)
                        cams.Add(camera);
                }
            }
        }

        /// <summary>
        /// Gets a room's identifier from a <see cref="UnityEngine.GameObject"/>.
        /// </summary>
        /// <param name="gameObject">The identifier's gameobject.</param>
        /// <returns>The room's identifier if found, otherwise null.</returns>
        public static RoomIdentifier GetId(GameObject gameObject)
        {
            if (gameObject == null)
                return null;

            RoomIdentifier room = null;

            if (gameObject.CompareTag("Player"))
            {
                Player player = PlayersList.Get(gameObject);

                if (player != null)
                {
                    if (player.Role == RoleType.Scp079)
                    {
                        room = player.Camera.GameObject.GetComponentInParent<RoomIdentifier>();
                    }
                }
            }
            else
            {
                room = gameObject.GetComponentInParent<RoomIdentifier>();
            }

            if (room == null)
            {
                if (Physics.RaycastNonAlloc(new Ray(gameObject.transform.position, Vector3.down), Map.cache, 10f, 1 << 0, QueryTriggerInteraction.Ignore) == 1)
                {
                    room = Map.cache[0].collider.gameObject.GetComponentInParent<RoomIdentifier>();
                }
            }

            return room;
        }

        /// <summary>
        /// Gets a room from it's unique ID.
        /// </summary>
        /// <param name="uId">The room's ID.</param>
        /// <returns>The room if found, otherwise null.</returns>
        public static Room Get(int uId)
        {
            var rId = RoomIdentifier.AllRoomIdentifiers.FirstOrDefault(x => x.UniqueId == uId);

            if (rId == null)
                return null;

            return Get(rId);
        }

        /// <summary>
        /// Gets a room from it's identifier.
        /// </summary>
        /// <param name="id">The room's identifier.</param>
        /// <returns>The room if found, otherwise null.</returns>
        public static Room Get(RoomIdentifier id)
        {
            Room room = null;

            if (!rooms.TryGetValue(id.UniqueId, out room))
                room = new Room(id, true);

            return room;
        }

        /// <summary>
        /// Gets a room from it's <see cref="UnityEngine.GameObject"/>.
        /// </summary>
        /// <param name="gameObject">The room's gameobject.</param>
        /// <returns>The room if found, otherwise null.</returns>
        public static Room Get(GameObject gameObject)
            => Get(GetId(gameObject));
    }
}