using System;
using System.Linq;

using InventorySystem;

using Atlas.Extensions;

using UnityEngine;

namespace Atlas.Entities
{
    /// <summary>
    /// A class for managing dummy players.
    /// </summary>
    public class Dummy
    {
        internal Player _player;
        internal DummyInfo? _spawnedWith;

        /// <summary>
        /// Initializes a new instance of the <see cref="Dummy"/> class.
        /// </summary>
        /// <param name="player">The dummy's player wrapper.</param>
        public Dummy(Player player)
        {
            if (player == null)
                throw new ArgumentNullException("player");

            if (PlayersList.List.Contains(player))
                throw new InvalidOperationException("You cannot create dummy objects on real players!");

            _player = player;

            PlayersList.dummyPlayers.Add(_player);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Dummy"/> class.
        /// </summary>
        /// <param name="dummySpawnInfo">The parameters to spawn the dummy with.</param>
        public Dummy(DummyInfo? dummySpawnInfo = null) : this(SpawnWithPlayer(dummySpawnInfo)) 
        {
            _spawnedWith = dummySpawnInfo;
        }

        /// <summary>
        /// Gets or set's the dummy's role.
        /// </summary>
        public RoleType Role { get => _player.Role; set => _player.Role = value; }

        /// <summary>
        /// Gets or sets the dummy's held item.
        /// </summary>
        public ItemType HeldItem { get => _player.CurrentItem?.Id ?? ItemType.None; set => _player.CurrentItem = new Items.Base.BaseItem(value); }

        /// <summary>
        /// Gets or sets the dummy's position.
        /// </summary>
        public Vector3 Position { get => _player.Position; set => _player.Position = value; }

        /// <summary>
        /// Gets or sets the dummy's scale.
        /// </summary>
        public Vector3 Scale { get => _player.Scale; set => _player.Scale = value; }

        /// <summary>
        /// Gets or sets the dummy's rotation.
        /// </summary>
        public Quaternion Rotation { get => _player.RotationsQ; set => _player.RotationsQ = value; }

        /// <summary>
        /// Gets the <see cref="Player"/> wrapper instance.
        /// </summary>
        public Player Controller { get => _player; }

        /// <summary>
        /// Gets the dummy's <see cref="global::ReferenceHub"/> component.
        /// </summary>
        public ReferenceHub ReferenceHub { get => _player?.Hub; }

        /// <summary>
        /// Gets or sets the dummy's nickname.
        /// </summary>
        public string Nick { get => _player.Nickname; set => _player.Nickname = value; }

        /// <summary>
        /// Gets or sets the dummy's User ID.
        /// </summary>
        public string UserId { get => _player.UserId; set => _player.UserId = value; }

        /// <summary>
        /// Gets or sets the dummy's IP address.
        /// </summary>
        public string IpAddress { get => _player.IpAddress; set => _player.Hub.queryProcessor._ipAddress = value; }

        /// <summary>
        /// Gets or sets the dummy's Player ID.
        /// </summary>
        public int PlayerId { get => _player.PlayerId; set => _player.PlayerId = value; }

        /// <summary>
        /// Gets the parameters this dummy was spawned with.
        /// </summary>
        public DummyInfo? SpawnInfo { get => _spawnedWith; }

        /// <summary>
        /// Despawns the dummy.
        /// </summary>
        public void Despawn()
            => _player.GameObject.UnSpawn();

        /// <summary>
        /// Spawns the dummy.
        /// </summary>
        public void Spawn()
            => _player.GameObject.Spawn();

        /// <summary>
        /// Destroys this dummy instance.
        /// </summary>
        public void Destroy()
        {
            PlayersList.dummyPlayers.Remove(_player);

            Mirror.NetworkServer.Destroy(_player.GameObject);

            _player = null;
        }

        /// <summary>
        /// Spawns a dummy and returns a dummy instance.
        /// </summary>
        /// <param name="dummySpawnInfo">The parameters to spawn the dummy with.</param>
        /// <returns>The dummy instance of the spawned dummy, can be null if an exception occured.</returns>
        public static Dummy Spawn(DummyInfo? dummySpawnInfo)
            => new Dummy(dummySpawnInfo);

        /// <summary>
        /// Spawns a dummy and returns an instance of the <see cref="Player"/> wrapper.
        /// </summary>
        /// <param name="dummySpawnInfo">The parameters to spawn the dummy with.</param>
        /// <returns>The dummy's <see cref="Player"/> wrapper, can be null if an exception occured.</returns>
        public static Player SpawnWithPlayer(DummyInfo? dummySpawnInfo = null)
        {
            var hub = SpawnWithHub(dummySpawnInfo);

            if (hub != null)
                return new Player(hub);
            else
                return null;
        }

        /// <summary>
        /// Spawns a dummy and returns it's <see cref="ReferenceHub"/> component.
        /// </summary>
        /// <param name="dummySpawnInfo">The parameters to spawn the dummy with.</param>
        /// <returns>The dummy's <see cref="ReferenceHub"/> component, can be null if an exception occured.</returns>
        public static ReferenceHub SpawnWithHub(DummyInfo? dummySpawnInfo = null)
        {
            if (!dummySpawnInfo.HasValue)
                dummySpawnInfo = DummyInfo.DefaultSpawnInfo;

            return SpawnWithHub(dummySpawnInfo.Value.Role, dummySpawnInfo.Value.HeldItem, dummySpawnInfo.Value.Inventory,
                dummySpawnInfo.Value.Position, dummySpawnInfo.Value.Scale, dummySpawnInfo.Value.Rotation,
                dummySpawnInfo.Value.Nickname, dummySpawnInfo.Value.UserId, dummySpawnInfo.Value.IpAddress,
                dummySpawnInfo.Value.PlayerId, dummySpawnInfo.Value.SpawnAutomatically);
        }

        /// <summary>
        /// Spawns a dummy and returns it's <see cref="ReferenceHub"/> component.
        /// </summary>
        /// <param name="role">The role to spawn the dummy as.</param>
        /// <param name="heldItem">The item the dummy will hold.</param>
        /// <param name="inventory">The dummy's inventory. <b>This MUST include the item the dummy's gonna hold.</b></param>
        /// <param name="position">The position where the dummy will spawn.</param>
        /// <param name="scale">The size of the dummy.</param>
        /// <param name="rotation">The dummy's model rotation.</param>
        /// <param name="nickname">The dummy's nickname.</param>
        /// <param name="userId">The dummy's User ID.</param>
        /// <param name="ipAddress">The dummy's IP Address.</param>
        /// <param name="playerId">The dummy's Player ID.</param>
        /// <param name="spawnAuto">Whether or not to spawn the dummy.</param>
        /// <returns>The dummy's <see cref="ReferenceHub"/> component, can be null if an exception occured.</returns>
        public static ReferenceHub SpawnWithHub(RoleType? role = null, ItemType? heldItem = null, ItemType[] inventory = null, Vector3? position = null,
            Vector3? scale = null, Quaternion? rotation = null, string nickname = null, string userId = null, string ipAddress = null
            , int? playerId = null, bool spawnAuto = true)
        {
            try
            {
                var prefab = Prefab.GetPrefab(Enums.PrefabType.Player);

                var clone = prefab.Copy();

                if (position.HasValue)
                    clone.transform.position = position.Value;

                if (scale.HasValue)
                    clone.transform.localScale = scale.Value;

                if (rotation.HasValue)
                    clone.transform.rotation = rotation.Value;

                var nick = string.IsNullOrWhiteSpace(nickname) ? DummyInfo.DefaultName : nickname;
                var usId = string.IsNullOrWhiteSpace(userId) ? DummyInfo.DefaultUserId : userId;
                var ip = string.IsNullOrWhiteSpace(ipAddress) ? DummyInfo.DefaultIpAddress : ipAddress;
                var pId = playerId.HasValue ? playerId.Value : DummyInfo.DefaultPlayerId;

                var hub = clone.GetComponent<ReferenceHub>();

                hub.characterClassManager.UserId = usId;
                hub.queryProcessor.PlayerId = pId;
                hub.queryProcessor._ipAddress = ip;
                hub.nicknameSync.SetNick(nick);

                if (role.HasValue)
                    hub.characterClassManager.SetClassIDAdv(role.Value, false, CharacterClassManager.SpawnReason.ForceClass, false);

                if (inventory != null && inventory.Length > 0 && inventory.Length < 9)
                {
                    foreach (var itemType in inventory)
                    {
                        hub.inventory.ServerAddItem(itemType);
                    }

                    hub.inventory.SendAmmoNextFrame = true;
                    hub.inventory.SendItemsNextFrame = true;
                }

                if (heldItem.HasValue && inventory != null && inventory.Contains(heldItem.Value))
                {
                    var item = hub.inventory.UserInventory.Items.Values.FirstOrDefault(x => x.ItemTypeId == heldItem.Value);

                    if (item != null)
                    {
                        hub.inventory.CurInstance = item;

                        hub.inventory.SendAmmoNextFrame = true;
                        hub.inventory.SendItemsNextFrame = true;
                    }
                }

                if (spawnAuto)
                    clone.Spawn();

                return hub;
            }
            catch (Exception e)
            {
                Log.Error("Atlas", $"An error occcured while spawning a dummy! (0)");
                Log.Error("Atlas", e);

                return null;
            }
        }
    }
}
