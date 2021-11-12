using System.Collections.Generic;

using MapGeneration.Distributors;

using UnityEngine;

using NorthwoodLib.Pools;

using Atlas.Extensions;

namespace Atlas.Entities
{
    /// <summary>
    /// A wrapper for <see cref="MapGeneration.Distributors.Locker"/>.
    /// </summary>
    public class Locker
    {
        internal MapGeneration.Distributors.Locker locker;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapGeneration.Distributors.Locker"/> class.
        /// </summary>
        /// <param name="locker"></param>
        internal Locker(MapGeneration.Distributors.Locker locker, bool addToApi = false)
        {
            this.locker = locker;

            Dictionary<int, Chamber> chamberDict = new Dictionary<int, Chamber>(locker.Chambers.Length);

            foreach (LockerChamber ch in locker.Chambers)
            {
                Chamber chamber = new Chamber(ch, true);

                chamberDict.Add(chamber.NetId, chamber);
            }

            Chambers = chamberDict;

            if (addToApi)
                Map.lockers.Add(this);
        }

        /// <summary>
        /// Gets a read-only dictionary of all chambers in this locker.
        /// </summary>
        public IReadOnlyDictionary<int, Chamber> Chambers { get; }

        /// <summary>
        /// Gets or sets the locker's loot.
        /// </summary>
        public LockerLoot[] Loot { get => locker.Loot; set => locker.Loot = value; }

        /// <summary>
        /// Gets the locker name.
        /// </summary>
        public string Name { get => locker.name; }

        /// <summary>
        /// Gets the locker's network ID.
        /// </summary>
        public uint NetId { get => locker.netId; }

        /// <summary>
        /// Gets the locker's <see cref="UnityEngine.GameObject"/>.
        /// </summary>
        public GameObject GameObject { get => locker.gameObject.gameObject; }

        /// <summary>
        /// Gets or sets the locker's position.
        /// </summary>
        public Vector3 Position { get => locker.transform.position; set => GameObject.Teleport(value); }

        /// <summary>
        /// Gets or sets the locker's rotation.
        /// </summary>
        public Quaternion Rotation { get => locker.transform.rotation; set => GameObject.Rotate(value); }

        /// <summary>
        /// Gets or sets the locker's scale.
        /// </summary>
        public Vector3 Scale { get => locker.transform.localScale; set => GameObject.Resize(value); }

        /// <summary>
        /// Spawns an item inside this locker.
        /// </summary>
        /// <param name="id">The <see cref="ItemType"/> to spawn.</param>
        /// <param name="amount">The item amount.</param>
        /// <param name="uses">The amount of uses. Setting it to -1 will make it default to <paramref name="amount"/>.</param>
        public void SpawnItem(ItemType id, int amount, int uses = -1)
            => SpawnItem(id, uses == -1 ? amount : uses, amount, 100);

        /// <summary>
        /// Spawns an item inside this locker.
        /// </summary>
        /// <param name="id">The ItemType to spawn.</param>
        /// <param name="uses">Max uses.</param>
        /// <param name="maxInChamber">Maximum amount of this item in one chamber.</param>
        /// <param name="chanceOfSpawn">Item's chance of spawning.</param>
        public void SpawnItem(ItemType id, int uses, int maxInChamber, int chanceOfSpawn)
        {
            List<LockerLoot> loot = ListPool<LockerLoot>.Shared.Rent(Loot);

            loot.Add(new LockerLoot
            {
                MaxPerChamber = maxInChamber,
                ProbabilityPoints = chanceOfSpawn,
                RemainingUses = uses,
                TargetItem = id
            });

            Loot = loot.ToArray();

            ListPool<LockerLoot>.Shared.Return(loot);
        }

        /// <summary>
        /// Deletes this locker.
        /// </summary>
        public void Delete()
            => locker.gameObject.Delete();

        /// <summary>
        /// Gets a chamber by it's network ID.
        /// </summary>
        /// <param name="id">The network ID.</param>
        /// <returns>The chamber found, if any.</returns>
        public Chamber GetChamber(int id) 
            => Chambers.TryGetValue(id, out Chamber chamber) ? chamber : null;

        /// <summary>
        /// Tries to find a <see cref="Locker"/> by a <see cref="global::Locker"/>. This method will ALWAYS return an instance as it creates a new item in case it wasn't found.
        /// </summary>
        /// <param name="locker"></param>
        /// <returns></returns>
        public static Locker Get(MapGeneration.Distributors.Locker locker)
        {
            foreach (Locker locke in Map.lockers)
            {
                if (locke.locker == locker || locke.NetId == locker.netId)
                    return locke;
            }

            return new Locker(locker, true);
        }
    }
}