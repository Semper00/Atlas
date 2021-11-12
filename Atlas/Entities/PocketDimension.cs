using NorthwoodLib.Pools;

using System.Collections.Generic;

using UnityEngine;

namespace Atlas.Entities
{
    /// <summary>
    /// Represents the in-game Pocket Dimension.
    /// </summary>
    public class PocketDimension
    {
        internal List<PocketDimensionExit> exits;

        public PocketDimension(Room room, Vector3 pos)
        {
            Position = pos;
            Room = room;

            exits = ListPool<PocketDimensionExit>.Shared.Rent();
        }

        ~PocketDimension()
            => ListPool<PocketDimensionExit>.Shared.Return(exits);

        /// <summary>
        /// Gets the dimension's room object.
        /// </summary>
        public Room Room { get; }

        /// <summary>
        /// Gets the pocket dimension's position.
        /// </summary>
        public Vector3 Position { get; }

        /// <summary>
        /// Gets the <see cref="IReadOnlyList{T}"/> list of all pocket dimension exits.
        /// </summary>
        public IReadOnlyList<PocketDimensionExit> Exits { get => exits; }

        internal void InternalRefreshPocketDimensionExits()
        {
            var list = Map.FindArray<PocketDimensionTeleport>();

            var notFound = ListPool<PocketDimensionTeleport>.Shared.Rent();

            foreach (PocketDimensionTeleport tp in list)
            {
                bool found = false;

                foreach (PocketDimensionExit ex in exits)
                {
                    if (ex.NetId == tp.netId)
                    {
                        found = true;

                        if (ex.exitChanged)
                            continue;

                        ex.InternalResetPocketDimensionExitType(tp._type);
                    }
                }

                if (!found)
                    notFound.Add(tp);
            }

            foreach (PocketDimensionTeleport ntp in notFound)
                exits.Add(new PocketDimensionExit(ntp));

            ListPool<PocketDimensionTeleport>.Shared.Return(notFound);
        }
    }
}
