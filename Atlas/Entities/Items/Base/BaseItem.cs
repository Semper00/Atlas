using System.Collections.Generic;

using InventorySystem.Items;

using UnityEngine;

using Atlas.Abstractions;
using Atlas.Entities.Pickups.Base;

namespace Atlas.Entities.Items.Base
{
    /// <summary>
    /// A base class for in-game items.
    /// </summary>
    public class BaseItem : MapObject
    {
        internal static List<BaseItem> items = new List<BaseItem>();

        internal BasePickup pickup;

        internal BaseItem(ItemBase itemBase, bool addToApi = false)
        {
            Base = itemBase;

            if (addToApi)
                AddItemToCollection(this);
        }

        internal BaseItem(ItemType type) : this(ReferenceHub.HostHub.inventory.CreateItemInstance(type, false)) { }

        /// <summary>
        /// Gets the base <see cref="ItemBase"/>.
        /// </summary>
        public ItemBase Base { get; }

        /// <summary>
        /// Gets the <see cref="ItemCategory"/>.
        /// </summary>
        public ItemCategory Category { get => Base.Category; }

        /// <summary>
        /// Gets or sets the item's ID.
        /// </summary>
        public ItemType Id { get => Base.ItemTypeId; set => Base.ItemTypeId = value; }

        /// <summary>
        /// Gets or sets the item's serial.
        /// </summary>
        public ushort Serial { get => Base.ItemSerial; set => Base.ItemSerial = value; }

        /// <summary>
        /// Gets the item's <see cref="UnityEngine.GameObject"/>.
        /// </summary>
        public GameObject GameObject { get => Base.gameObject; }

        /// <summary>
        /// Gets the item's <see cref="Entities.BasePickup"/>.
        /// </summary>
        public BasePickup Pickup { get => pickup == null ? (pickup = new BasePickup(Base.PickupDropModel, true)) : pickup; }

        /// <summary>
        /// Gets or sets the item's <see cref="ItemThrowSettings"/>.
        /// </summary>
        public ItemThrowSettings ThrowSettings { get => Base.ThrowSettings; set => Base.ThrowSettings = value; }

        /// <summary>
        /// Gets or sets the item's <see cref="ItemTierFlags"/>.
        /// </summary>
        public ItemTierFlags TierFlags { get => Base.TierFlags; set => Base.TierFlags = value; }

        /// <inheritdoc/>
        public override uint NetId { get => (uint)Base.GetInstanceID(); }

        /// <summary>
        /// Gets the item's owner.
        /// </summary>
        public Player Owner { get => PlayersList.Get(Base.Owner); }

        /// <summary>
        /// Drops this item at the owner's position.
        /// </summary>
        /// <returns>The dropped pickup.</returns>
        public BasePickup Drop(bool removeFromInventory = true)
        {
            pickup = new BasePickup(Base.PickupDropModel, true);

            var owner = Owner;

            if (owner != null)
            {
                pickup.Spawn(owner.Position);

                if (removeFromInventory)
                {
                    owner.Hub.inventory.UserInventory.Items.Remove(Serial);
                    owner.Hub.inventory.SendItemsNextFrame = true;
                    owner.Hub.inventory.SendAmmoNextFrame = true;
                }
            }
            else
            {
                pickup.Spawn();
            }

            return pickup;
        }

        /// <summary>
        /// Drops this item at a custom position.
        /// </summary>
        /// <param name="position">The position to spawn the item at.</param>
        /// <returns>The dropped pickup.</returns>
        public BasePickup Drop(Vector3 position, bool removeFromInventory = true)
        {
            pickup = new BasePickup(Base.PickupDropModel, true);

            var owner = Owner;

            if (owner != null)
            {
                if (removeFromInventory)
                {
                    owner.Hub.inventory.UserInventory.Items.Remove(Serial);
                    owner.Hub.inventory.SendItemsNextFrame = true;
                    owner.Hub.inventory.SendAmmoNextFrame = true;
                }

                pickup.Spawn(position);
            }
            else
            {
                pickup.Spawn(position);
            }

            return pickup;
        }

        /// <summary>
        /// Returns this item as a specified type.
        /// </summary>
        /// <typeparam name="TITem">The item to return as.</typeparam>
        /// <returns>The converted item if it's type matches, otherwise null.</returns>
        public TITem As<TITem>() where TITem : BaseItem
        {
            if (this is TITem item)
            {
                return item;
            }
            else
            {
                return default;
            }
        }

        /// <summary>
        /// Returns this item as a specified type.
        /// </summary>
        /// <typeparam name="TITem">The item to return as.</typeparam>
        /// <returns>The converted item if it's type matches, otherwise null.</returns>
        public TITem AsBase<TITem>() where TITem : ItemBase
        {
            if (Base is TITem item)
            {
                return item;
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
        public static BaseItem Get(ItemBase item)
        {
            int index = FindIndex(item);

            if (index != -1)
                return items[index];

            return new BaseItem(item, true);
        }

        /// <summary>
        /// Gets an item instance from an item type.
        /// </summary>
        /// <param name="type">The item type to create.</param>
        /// <returns>The item instance.</returns>
        public static BaseItem Get(ItemType type)
            => new BaseItem(type);

        /// <summary>
        /// Creates a new item from an <see cref="ItemType"/>.
        /// </summary>
        /// <typeparam name="TItem">The item type.</typeparam>
        /// <param name="type">The <see cref="ItemType"/>.</param>
        /// <returns>The item created.</returns>
        public static TItem Get<TItem>(ItemType type) where TItem : BaseItem
            => new BaseItem(type).As<TItem>();

        #region Internal Collection Management

        internal static void AddItemToCollection(BaseItem item)
            => items.Add(item);

        internal static void AddItemToCollection(ItemBase itemBase)
            => items.Add(new BaseItem(itemBase));

        internal static bool RemoveInCollection(BaseItem item)
        {
            int index = FindIndex(item);

            if (index != -1)
            {
                items.RemoveAt(index);

                return true;
            }

            return false;
        }

        internal static bool RemoveInCollection(ItemBase item)
        {
            int index = FindIndex(item);

            if (index != -1)
            {
                items.RemoveAt(index);

                return true;
            }

            return false;
        }

        internal static int FindIndex(BaseItem item)
        {
            for (int i = 0; i < items.Count; i++)
            {
                BaseItem it = items[i];

                if (it.NetId == item.NetId
                    || it.Serial == item.Serial)
                    return i;
            }

            return -1;
        }

        internal static int FindIndex(ItemBase item)
        {
            for (int i = 0; i < items.Count; i++)
            {
                BaseItem it = items[i];

                if (it.NetId == item.GetInstanceID()
                    || it.Serial == item.ItemSerial)
                    return i;
            }

            return -1;
        }

        #endregion
    }
}
