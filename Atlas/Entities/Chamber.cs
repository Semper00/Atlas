using MapGeneration.Distributors;

using Interactables.Interobjects.DoorUtils;

using Atlas.Extensions;

using UnityEngine;

using System;
using System.Collections.Generic;

using InventorySystem.Items.Pickups;

namespace Atlas.Entities
{
    /// <summary>
    /// A wrapper for <see cref="LockerChamber"/>.
    /// </summary>
    public class Chamber
    { 
        internal LockerChamber chamber;

        /// <summary>
        /// Initialzes a new instance of the <see cref="Chamber"/> class.
        /// </summary>
        /// <param name="chamber">The original chamber.</param>
        /// <param name="locker">The locker this chamber is located in.</param>
        /// <param name="chamberId">The ID of this chamber.</param>
        internal Chamber(LockerChamber chamber, bool addToApi = false)
        {
            Locker = FindLocker(chamber);

            this.chamber = chamber;

            if (addToApi)
                Map.chambers.Add(this);
        }

        /// <summary>
        /// Gets the <see cref="Entities.Locker"/> this chamber is located in.
        /// </summary>
        public Locker Locker { get; }

        /// <summary>
        /// Gets the chamber's <see cref="UnityEngine.GameObject"/>.
        /// </summary>
        public GameObject GameObject { get => chamber.gameObject; }

        /// <summary>
        /// Gets the base object.
        /// </summary>
        public LockerChamber Base { get => chamber; }

        /// <summary>
        /// Gets or sets the chamber's spawnpoint.
        /// </summary>
        public Transform Spawnpoint { get => chamber._spawnpoint; set => chamber._spawnpoint = value; }

        /// <summary>
        /// Gets a <see cref="HashSet{T}"/> with all items.
        /// </summary>
        public HashSet<ItemPickupBase> Items { get => chamber._content; }

        /// <summary>
        /// Gets the chamber's network ID.
        /// </summary>
        public int NetId { get => chamber.GetInstanceID(); }

        /// <summary>
        /// Gets or sets an array of items that this chamber accepts.
        /// </summary>
        public ItemType[] AcceptedItems { get => chamber.AcceptableItems ?? Array.Empty<ItemType>(); set => chamber.AcceptableItems = value; }

        /// <summary>
        /// Gets the chamber's name.
        /// </summary>
        public string Name { get => chamber.name; }

        /// <summary>
        /// Gets a value indicating whether or not players can interact.
        /// </summary>
        public bool CanInteract { get => chamber.CanInteract; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the chamber is open.
        /// </summary>
        public bool IsOpen { get => chamber.IsOpen; set => chamber.IsOpen = value; }

        /// <summary>
        /// Gets a value indicating whether this chamber was opened or not.
        /// </summary>
        public bool WasOpened { get => chamber._wasEverOpened; }

        /// <summary>
        /// Gets or sets the chamber's access token.
        /// </summary>
        public KeycardPermissions Permissions { get => chamber.RequiredPermissions; set => chamber.RequiredPermissions = value; }

        /// <summary>
        /// Spawns an item inside this chamber.
        /// </summary>
        /// <param name="id">The Item Type to spawn.</param>
        /// <param name="amount">The amount to spawn.</param>
        public void SpawnItem(ItemType id, int amount = 1)
            => chamber.SpawnItem(id, amount);

        /// <summary>
        /// Gets a <see cref="Chamber"/> from a <see cref="LockerChamber"/>. This method will ALWAYS return an instance as it creates a new item in case it wasn't found.
        /// </summary>
        /// <param name="chamber">The <see cref="LockerChamber"/> to search.</param>
        /// <returns>The <see cref="Chamber"/> instance found.</returns>
        public static Chamber Get(LockerChamber chamber)
        {
            foreach (Chamber cham in Map.chambers)
            {
                if (cham.chamber == chamber || cham.NetId == chamber.GetId())
                    return cham;
            }

            return new Chamber(chamber, true);
        }

        /// <summary>
        /// Tries to find a <see cref="Chamber"/> by it's network ID.
        /// </summary>
        /// <param name="netId">The network ID to search by. You can retrieve it by using <see cref="MapExtensions.GetId(LockerChamber)"/></param>.
        /// <returns>The <see cref="Chamber"/> found, if any.</returns>
        public static Chamber Get(int netId)
        {
            foreach (Chamber cham in Map.chambers)
            {
                if (cham.NetId == netId)
                    return cham;
            }

            return null;
        }

        internal static Locker FindLocker(LockerChamber cham)
        {
            foreach (Locker locker in Map.lockers)
            {
                foreach (LockerChamber chamber in locker.locker.Chambers)
                {
                    if (cham == chamber || cham.GetId() == chamber.GetId())
                        return locker;
                }
            }

            return null;
        }
    }
}