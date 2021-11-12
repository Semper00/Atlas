using UnityEngine;

namespace Atlas.Entities
{
    /// <summary>
    /// Serves as an easier way for spawning dummies.
    /// </summary>
    public struct DummyInfo
    {
        /// <summary>
        /// The name used for dummies if you don't specify one when spawning a dummy.
        /// </summary>
        public const string DefaultName = "SCP-079";

        /// <summary>
        /// The User ID used for dummies if you don't specify one when spawning a dummy. 
        /// </summary>
        public const string DefaultUserId = "SCP-079";

        /// <summary>
        /// The IP Address used for dummies if you don't specify one when spawning a dummy.
        /// </summary>
        public const string DefaultIpAddress = "127.0.0.WAN";

        /// <summary>
        /// The Player ID used for dummies if you don't specify one when spawning a dummy.
        /// </summary>
        public const int DefaultPlayerId = 9999;

        /// <summary>
        /// Position where the dummy will spawn.
        /// </summary>
        public Vector3? Position;

        /// <summary>
        /// The size of the dummy.
        /// </summary>
        public Vector3? Scale;

        /// <summary>
        /// The rotation of the dummy's model.
        /// </summary>
        public Quaternion? Rotation;

        /// <summary>
        /// The role the dummy will spawn as.
        /// </summary>
        public RoleType? Role;

        /// <summary>
        /// The item the dummy will hold when it spawns.
        /// </summary>
        public ItemType? HeldItem;

        /// <summary>
        /// The dummy's full inventory.
        /// </summary>
        public ItemType[] Inventory;

        /// <summary>
        /// The dummy's nickname.
        /// </summary>
        public string Nickname;

        /// <summary>
        /// The dummy's User ID.
        /// </summary>
        public string UserId;

        /// <summary>
        /// The dummy's IP Address.
        /// </summary>
        public string IpAddress;

        /// <summary>
        /// The dummy's Player ID.
        /// </summary>
        public int PlayerId;

        /// <summary>
        /// Will spawn the dummy automatically if set to true. When set to false, you'll have to spawn it yourself.
        /// </summary>
        public bool SpawnAutomatically;

        /// <summary>
        /// Spawns a dummy and returns it's <see cref="ReferenceHub"/> component.
        /// </summary>
        /// <returns>The dummy's <see cref="ReferenceHub"/> component, can be null if an exception occured.</returns>
        public ReferenceHub SpawnWithHub()
            => Dummy.SpawnWithHub(this);

        public Player SpawnWithPlayer()
            => Dummy.SpawnWithPlayer(this);

        /// <summary>
        /// Gets the default spawning info for dummies.
        /// </summary>
        public static readonly DummyInfo DefaultSpawnInfo = new DummyInfo
        {
            HeldItem = ItemType.None,
            Inventory = null,
            IpAddress = DefaultIpAddress,
            Nickname = DefaultName,
            PlayerId = DefaultPlayerId,
            Position = Vector3.zero,
            Role = RoleType.None,
            Rotation = Quaternion.identity,
            Scale = Vector3.one,
            UserId = DefaultUserId,
            SpawnAutomatically = true
        };
    }
}
