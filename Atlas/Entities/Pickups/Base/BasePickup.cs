using UnityEngine;

using Mirror;

using InventorySystem;
using InventorySystem.Items;
using InventorySystem.Items.Pickups;

using Atlas.Abstractions;
using Atlas.Extensions;

using System.Collections.Generic;

namespace Atlas.Entities.Pickups.Base
{
    /// <summary>
    /// Class for dropped items management.
    /// </summary>
    public class BasePickup : NetworkObject
    {
        internal static List<BasePickup> pickups = new List<BasePickup>();

        internal BasePickup() { }

        internal BasePickup(ItemPickupBase pickupBase, bool addToApi = false)
        {
            Base = pickupBase;

            if (addToApi)
                pickups.Add(this);
        }

        public BasePickup(ItemType type) : this(ReferenceHub.HostHub.inventory.CreateItemInstance(type, false).PickupDropModel, true) { }

        /// <inheritdoc/>
        public override uint NetId { get => Base.netId; }

        /// <summary>
        /// Gets the item's base <see cref="ItemPickupBase"/>.
        /// </summary>
        public ItemPickupBase Base { get; internal set; }

        /// <summary>
        /// Gets the item's <see cref="UnityEngine.GameObject"/>.
        /// </summary>
        public override GameObject GameObject { get => Base.gameObject; }

        /// <summary>
        /// Gets the item's <see cref="UnityEngine.Rigidbody"/>.
        /// </summary>
        public Rigidbody Rigidbody { get => Base.Rb; }

        /// <summary>
        /// Gets the item's owner. Possibly null.
        /// </summary>
        public Player Owner { get => PlayersList.Get(Base.PreviousOwner.Hub); }

        /// <summary>
        /// Gets the item's <see cref="IPickupPhysicsModule"/>.
        /// </summary>
        public IPickupPhysicsModule Physics { get => Base.PhysicsModule; }

        /// <summary>
        /// Gets or sets the item's <see cref="PickupSyncInfo"/>.
        /// </summary>
        public PickupSyncInfo Info { get => Base.NetworkInfo; set => Base.NetworkInfo = value; }

        /// <summary>
        /// Gets or sets the item's position.
        /// </summary>
        public override Vector3 Position
        {
            get => GameObject.transform.position;
            set
            {
                GameObject.transform.position = value;

                Base.RefreshPositionAndRotation();
            }
        }

        /// <summary>
        /// Gets or sets the item's scale.
        /// </summary>
        public override Vector3 Scale { get => GameObject.transform.localScale; set => GameObject.Resize(value); }

        /// <summary>
        /// Gets or sets the item's rotation.
        /// </summary>
        public override Quaternion Rotation
        {
            get => GameObject.transform.rotation;
            set
            {
                GameObject.transform.rotation = value;

                Base.RefreshPositionAndRotation();
            }
        }

        /// <summary>
        /// Gets or sets the item's ID.
        /// </summary>
        public ItemType Id
        {
            get => Info.ItemId;
            set
            {
                if (!InventoryItemLoader.AvailableItems.TryGetValue(value, out ItemBase item))
                    item = ReferenceHub.HostHub.inventory.ServerAddItem(value, Serial, Base);

                ItemPickupBase pickup = item.PickupDropModel;

                pickup.transform.position = Position;
                pickup.transform.rotation = Rotation;
                pickup.RefreshPositionAndRotation();

                var info = Info;

                info.ItemId = value;

                pickup.NetworkInfo = info;

                Base.DestroySelf();

                Base = pickup;
            }
        }

        /// <summary>
        /// Gets or sets the item's serial.
        /// </summary>
        public ushort Serial
        {
            get => Info.Serial;
            set
            {
                var info = Info;

                info.Serial = value;

                Info = info;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether someone is picking up this pickup or not.
        /// </summary>
        public bool IsBeingUsed
        {
            get => Info.InUse;
            set
            {
                var info = Info;

                info.InUse = value;

                Info = info;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this pickup can be picked up or not.
        /// </summary>
        public bool IsLocked
        {
            get => Info.Locked;
            set
            {
                var info = Info;

                info.Locked = value;

                Info = info;
            }
        }

        /// <summary>
        /// Gets or sets the pickup's weight.
        /// </summary>
        public float Weight
        {
            get => Info.Weight;
            set
            {
                var info = Info;

                info.Weight = value;

                Info = info;
            }
        }

        /// <summary>
        /// Gets the pickup's picking duration.
        /// </summary>
        public float SearchTime { get => Base.SearchTime; }

        /// <summary>
        /// Deletes this pickup.
        /// </summary>
        public override void Delete()
            => Base.DestroySelf();

        /// <summary>
        /// Spawns this item.
        /// </summary>
        public void Spawn()
            => NetworkServer.Spawn(GameObject);

        /// <summary>
        /// Spawns this item.
        /// </summary>
        public void Spawn(Vector3 pos)
        {
            Position = pos;

            Spawn();
        }

        /// <summary>
        /// Spawns this item.
        /// </summary>
        public void Spawn(Vector3 pos, Vector3 scale)
        {
            Scale = scale;

            Spawn(pos);
        }

        /// <summary>
        /// Spawns this item.
        /// </summary>
        public void Spawn(Vector3 pos, Quaternion rot)
        {
            Rotation = rot;

            Spawn(pos);
        }

        /// <summary>
        /// Spawns this item.
        /// </summary>
        public void Spawn(Vector3 pos, Vector3 scale, Quaternion rot)
        {
            Rotation = rot;
            Scale = scale;

            Spawn(pos);
        }

        /// <summary>
        /// Despawns this item.
        /// </summary>
        public void Unspawn()
            => NetworkServer.UnSpawn(GameObject);

        /// <summary>
        /// Returns pickup item as a specified type.
        /// </summary>
        /// <typeparam name="TITem">The pickup to return as.</typeparam>
        /// <returns>The converted pickup if it's type matches, otherwise null.</returns>
        public TPickup As<TPickup>() where TPickup : BasePickup
        {
            if (this is TPickup pickup)
            {
                return pickup;
            }
            else
            {
                return default;
            }
        }

        /// <summary>
        /// Returns this pickup as a specified type.
        /// </summary>
        /// <typeparam name="TPickup">The pickup to return as.</typeparam>
        /// <returns>The converted pickup if it's type matches, otherwise null.</returns>
        public TPickup AsBase<TPickup>() where TPickup : ItemPickupBase
        {
            if (Base is TPickup pickup)
            {
                return pickup;
            }
            else
            {
                return default;
            }
        }

        /// <summary>
        /// Gets a <see cref="BaseItem"/> from a <see cref="ItemBase"/>.
        /// </summary>
        /// <param name="item">The item to find.</param>
        /// <returns>The item found.</returns>
        public static BasePickup Get(ItemPickupBase item)
        {
            int index = FindIndex(item);

            if (index != -1)
                return pickups[index];

            return new BasePickup(item, true);
        }

        /// <summary>
        /// Gets a new pickup instance.
        /// </summary>
        /// <param name="type">The item type of the pickup.</param>
        /// <returns>The pickup created.</returns>
        public static BasePickup Get(ItemType type)
            => new BasePickup(type);

        /// <summary>
        /// Creates a new item from an <see cref="ItemType"/>.
        /// </summary>
        /// <typeparam name="TItem">The item type.</typeparam>
        /// <param name="type">The <see cref="ItemType"/>.</param>
        /// <returns>The item created.</returns>
        public static TPickup Get<TPickup>(ItemType type) where TPickup : BasePickup
            => new BasePickup(type) as TPickup;

        internal static void AddItemToCollection(BasePickup item)
            => pickups.Add(item);

        internal static void AddItemToCollection(ItemPickupBase itemBase)
            => pickups.Add(new BasePickup(itemBase));

        internal static bool RemoveInCollection(BasePickup item)
        {
            int index = FindIndex(item);

            if (index != -1)
            {
                pickups.RemoveAt(index);

                return true;
            }

            return false;
        }

        internal static bool RemoveInCollection(ItemPickupBase item)
        {
            int index = FindIndex(item);

            if (index != -1)
            {
                pickups.RemoveAt(index);

                return true;
            }

            return false;
        }

        internal static int FindIndex(BasePickup item)
        {
            for (int i = 0; i < pickups.Count; i++)
            {
                BasePickup p = pickups[i];

                if (p.NetId == item.NetId
                    || p.Serial == item.Serial)
                    return i;
            }

            return -1;
        }

        internal static int FindIndex(ItemPickupBase item)
        {
            for (int i = 0; i < pickups.Count; i++)
            {
                BasePickup p = pickups[i];

                if (p.NetId == item.GetInstanceID()
                    || p.Serial == item.Info.Serial)
                    return i;
            }

            return -1;
        }
    }
}