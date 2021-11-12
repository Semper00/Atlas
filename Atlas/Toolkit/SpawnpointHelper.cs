using System.Collections.Generic;

using UnityEngine;

using Atlas.Entities;

namespace Atlas.Toolkit
{
    /// <summary>
    /// A class to help with handling spawnpoints.
    /// </summary>
    public static class SpawnpointHelper
    {
        /// <summary>
        /// A dictionary of RoleTypes and their respective GameObject names.
        /// </summary>
        public static IReadOnlyDictionary<RoleType, string> Names { get; } = new Dictionary<RoleType, string>
        {
            [RoleType.Scp106] = "SP_106",
            [RoleType.Scp049] = "SP_049",
            [RoleType.Scp079] = "SP_079",
            [RoleType.Scp096] = "SCP_096",
            [RoleType.Scp93953] = "SCP_939",
            [RoleType.Scp93989] = "SCP_939",
            [RoleType.Scp173] = "SP_173",
            [RoleType.FacilityGuard] = "SP_GUARD",
            [RoleType.NtfCaptain] = "SP_MTF",
            [RoleType.NtfPrivate] = "SP_MTF",
            [RoleType.NtfSergeant] = "SP_MTF",
            [RoleType.NtfSpecialist] = "SP_MTF",
            [RoleType.ChaosConscript] = "SP_CI",
            [RoleType.ChaosMarauder] = "SP_CI",
            [RoleType.ChaosRepressor] = "SP_CI",
            [RoleType.ChaosRifleman] = "SP_CI",
            [RoleType.Scientist] = "SP_RSC",
            [RoleType.ClassD] = "SP_CDP",
            [RoleType.Tutorial] = "TUT Spawn",
            [RoleType.Spectator] = null,
            [RoleType.None] = null
        };

        /// <summary>
        /// Gets all possible spawnpoints for a role.
        /// </summary>
        /// <param name="role">The role to get spawnpoints for.</param>
        /// <returns>A list of found spawnpoints.</returns>
        public static List<Vector3> GetSpawnpoints(RoleType role)
        {
            var name = Names[role];

            if (name == null)
                return null;

            var goL = GameObject.FindGameObjectsWithTag(name);

            List<Vector3> pos = new List<Vector3>(goL.Length);

            foreach (var go in GameObject.FindGameObjectsWithTag(name))
                pos.Add(go.transform.position);

            return pos;
        }

        /// <summary>
        /// Gets a single random spawnpoint for a role.
        /// </summary>
        /// <param name="role">The role to get a spawnpoint for.</param>
        /// <returns>The role's spawnpoint.</returns>
        public static Vector3 GetRandomSpawnpoint(RoleType role)
        {
            var sp = GetSpawnpoints(role);

            if (sp == null)
                return default;

            return sp[Server.Random.Next(sp.Count)];
        }

        /// <summary>
        /// Gets a dictionary of spawnpoints for all roles.
        /// </summary>
        /// <returns>The dictionary.</returns>
        public static Dictionary<RoleType, List<Vector3>> GetAll()
        {
            var roleTypes = EnumHelper<RoleType>.All;
            var dict = new Dictionary<RoleType, List<Vector3>>();

            foreach (var roleType in roleTypes)
            {
                if (!dict.ContainsKey(roleType))
                    dict.Add(roleType, null);

                if (roleType == RoleType.None || roleType == RoleType.Spectator)
                    continue;

                if (dict[roleType] == null)
                    dict[roleType] = new List<Vector3>();

                dict[roleType].AddRange(GetSpawnpoints(roleType));
            }

            return dict;
        }

        /// <summary>
        /// Fills a dictionary with spawnpoints.
        /// </summary>
        /// <param name="dict">The dictionary to fill.</param>
        public static void Fill(Dictionary<RoleType, List<Vector3>> dict)
        {
            if (dict == null)
                return;

            var roleTypes = EnumHelper<RoleType>.All;

            dict.Clear();

            foreach (var roleType in roleTypes)
            {
                if (!dict.ContainsKey(roleType))
                    dict.Add(roleType, null);

                if (roleType == RoleType.None || roleType == RoleType.Spectator)
                    continue;

                if (dict[roleType] == null)
                    dict[roleType] = new List<Vector3>();

                dict[roleType].AddRange(GetSpawnpoints(roleType));
            }
        }
    }
}
