using System;
using System.Collections.Generic;
using System.Linq;

using Atlas.Enums;
using Atlas.Extensions;

using UnityEngine;
using Mirror;

using Object = UnityEngine.Object;

namespace Atlas.Entities
{
    /// <summary>
    /// Used to manage spawnable prefabs.
    /// </summary>
    public class Prefab
    {
        internal static List<Prefab> allPrefabs;

        /// <summary>
        /// Gets the prefab's <see cref="UnityEngine.GameObject"/>.
        /// </summary>
        public GameObject GameObject { get; }

        /// <summary>
        /// Gets the prefab's ID which can be used in <see cref="NetworkClient.prefabs"/>.
        /// </summary>
        public Guid? ID { get; }

        /// <summary>
        /// Gets the prefab's type.
        /// </summary>
        public PrefabType Type { get; }

        /// <summary>
        /// Gets the prefab's name.
        /// </summary>
        public string Name { get; }

        internal Prefab(KeyValuePair<Guid, GameObject> prefab)
        {
            GameObject = prefab.Value;
            ID = prefab.Key;
            Type = GetPrefabType(prefab.Value.name);
            Name = prefab.Value.name;
        }

        internal Prefab(GameObject prefab)
        {
            if (prefab == null)
                return;

            GameObject = prefab;
            ID = null;
            Type = GetPrefabType(prefab.name);
            Name = prefab.name;
        }

        /// <summary>
        /// Gets a component from the prefab's game object.
        /// </summary>
        /// <typeparam name="TComponent">The component to get.</typeparam>
        /// <returns>The component instance if found, otherwise null.</returns>
        public TComponent GetComponent<TComponent>() where TComponent : Component
            => GameObject.TryGetComponent(out TComponent comp) ? comp : default;

        /// <summary>
        /// Gets all available prefabs.
        /// </summary>
        public static IReadOnlyList<Prefab> Prefabs { get => allPrefabs; }

        /// <summary>
        /// Spawns this prefab with the specified parameters.
        /// </summary>
        /// <param name="position">The position top spawn at.</param>
        /// <param name="scale">The scale to spawn with.</param>
        /// <param name="rotation">The rotation to spawn with.</param>
        /// <param name="spawn">Whether or not to spawn the object.</param>
        /// <returns>The spawned object.</returns>
        public GameObject Spawn(Vector3? position = null, Vector3? scale = null, Quaternion? rotation = null, bool spawn = false)
        {
            GameObject clone = Copy();

            if (position.HasValue)
                clone.transform.position = position.Value;

            if (scale.HasValue)
                clone.transform.localScale = scale.Value;

            if (rotation.HasValue)
                clone.transform.rotation = rotation.Value;

            if (spawn)
                clone.Spawn();

            return clone;
        }

        /// <summary>
        /// Spawns this prefab with the specified parameters.
        /// </summary>
        /// <param name="position">The position top spawn at.</param>
        /// <param name="scale">The scale to spawn with.</param>
        /// <param name="rotation">The rotation to spawn with.</param>
        /// <param name="spawn">Whether or not to spawn the object.</param>
        /// <returns>The spawned object.</returns>
        public TObject Spawn<TObject>(Vector3? position = null, Vector3? scale = null, Quaternion? rotation = null, bool spawn = false) where TObject : Component
        {
            TObject clone = Copy<TObject>();

            if (position.HasValue)
                clone.transform.position = position.Value;

            if (scale.HasValue)
                clone.transform.localScale = scale.Value;

            if (rotation.HasValue)
                clone.transform.rotation = rotation.Value;

            if (spawn)
                clone.gameObject.Spawn();

            return clone;
        }

        /// <summary>
        /// Instantiates a new instance of this prefab.
        /// </summary>
        /// <returns>The created instance.</returns>
        public GameObject Copy()
            => Object.Instantiate(GameObject);

        /// <summary>
        /// Instantiates a new instance of this prefab as the specified type.
        /// </summary>
        /// <typeparam name="TObject">The type to copy as.</typeparam>
        /// <returns>The created instance.</returns>
        public TObject Copy<TObject>() where TObject : Component
            => Object.Instantiate(GameObject.GetComponent<TObject>());

        /// <summary>
        /// Gets a prefab by it's type.
        /// </summary>
        /// <param name="type">The type to find.</param>
        /// <returns></returns>
        public static Prefab GetPrefab(PrefabType type)
            => allPrefabs.First(x => x.Type == type);

        /// <summary>
        /// Gets the prefab type from a gameobject's name.
        /// </summary>
        /// <param name="name">The GameObject name.</param>
        /// <returns>The prefab's type.</returns>
        public static PrefabType GetPrefabType(string name)
        {
            if (!Enum.TryParse(name, out PrefabType type))
            {
                switch (name)
                {
                    case "Work Station":
                        type = PrefabType.WorkStation;
                        break;
                    case "SCP-173_Ragdoll":
                        type = PrefabType.Ragdoll_SCP173;
                        break;
                    case "SCP-106_Ragdoll":
                        type = PrefabType.Ragdoll_SCP106;
                        break;
                    case "SCP-096_Ragdoll":
                        type = PrefabType.Ragdoll_SCP096;
                        break;
                    case "SCP-939-53_Ragdoll":
                        type = PrefabType.Ragdoll_SCP939_53;
                        break;
                    case "SCP-939-89_Ragdoll":
                        type = PrefabType.Ragdoll_SCP939_89;
                        break;
                    case "Grenade Flash":
                        type = PrefabType.GrenadeFlash;
                        break;
                    case "Grenade Frag":
                        type = PrefabType.GrenadeFrag;
                        break;
                    case "Grenade SCP-018":
                        type = PrefabType.GrenadeSCP018;
                        break;
                    case "EZ BreakableDoor":
                        type = PrefabType.EZ_BreakableDoor;
                        break;
                    case "HCZ BreakableDoor":
                        type = PrefabType.HCZ_BreakableDoor;
                        break;
                    case "LCZ BreakableDoor":
                        type = PrefabType.LCZ_BreakableDoor;
                        break;
                    case "sportTargetPrefab":
                        type = PrefabType.SportTarget;
                        break;
                    case "dboyTargetPrefab":
                        type = PrefabType.DboyTarget;
                        break;
                    case "binaryTargetPrefab":
                        type = PrefabType.BinaryTarget;
                        break;
                    case "TantrumObj":
                        type = PrefabType.Tantrum;
                        break;
                    case "Light Armor Pickup":
                        type = PrefabType.LightArmorPickup;
                        break;
                    case "Heavy Armor Pickup":
                        type = PrefabType.HeavyArmorPickup;
                        break;
                    case "Combat Armor Pickup":
                        type = PrefabType.CombatArmorPickup;
                        break;
                    case "Scp268PedestalStructure Variant":
                        type = PrefabType.Scp268PedestalStructure;
                        break;
                    case "Scp207PedestalStructure Variant":
                        type = PrefabType.Scp207PedestalStructure;
                        break;
                    case "Scp500PedestalStructure Variant":
                        type = PrefabType.Scp500PedestalStructure;
                        break;
                    case "Scp018PedestalStructure Variant":
                        type = PrefabType.Scp018PedestalStructure;
                        break;
                    case "Spawnable Work Station Structure":
                        type = PrefabType.WorkstationStructure;
                        break;
                    case "Scp2176PedestalStructure Variant":
                        type = PrefabType.Scp2176PedestalStructure;
                        break;
                    default:
                        type = PrefabType.Portal106;
                        break;
                
                }
            }

            return type;
        }

        /// <summary>
        /// Gets the fixed prefab gameobject name.
        /// </summary>
        /// <param name="type">The prefab type to get a name of.</param>
        /// <returns>The fixed name.</returns>
        public static string GetFixedName(PrefabType type)
        {
            switch (type)
            {
                case PrefabType.WorkStation:
                    return "Work Station";
                case PrefabType.Ragdoll_SCP173:
                    return "SCP-173_Ragdoll";
                case PrefabType.Ragdoll_SCP106:
                    return "SCP-106_Ragdoll";
                case PrefabType.Ragdoll_SCP096:
                    return "SCP-096_Ragdoll";
                case PrefabType.Ragdoll_SCP939_53:
                    return "SCP-939-53_Ragdoll";
                case PrefabType.Ragdoll_SCP939_89:
                    return "SCP-939-89_Ragdoll";
                case PrefabType.GrenadeFlash:
                    return "Grenade Flash";
                case PrefabType.GrenadeFrag:
                    return "Grenade Frag";
                case PrefabType.GrenadeSCP018:
                    return "Grenade SCP-018";
                case PrefabType.EZ_BreakableDoor:
                    return "EZ BreakableDoor";
                case PrefabType.HCZ_BreakableDoor:
                    return "HCZ BreakableDoor";
                case PrefabType.LCZ_BreakableDoor:
                    return "LCZ BreakableDoor";
                case PrefabType.SportTarget:
                    return "sportTargetPrefab";
                case PrefabType.DboyTarget:
                    return "dboyTargetPrefab";
                case PrefabType.BinaryTarget:
                    return "binaryTargetPrefab";
                case PrefabType.Tantrum:
                    return "TantrumObj";
                case PrefabType.LightArmorPickup:
                    return "Light Armor Pickup";
                case PrefabType.HeavyArmorPickup:
                    return "Heavy Armor Pickup";
                case PrefabType.CombatArmorPickup:
                    return "Combat Armor Pickup";
                case PrefabType.Scp268PedestalStructure:
                    return "Scp268PedestalStructure Variant";
                case PrefabType.Scp207PedestalStructure:
                    return "Scp207PedestalStructure Variant";
                case PrefabType.Scp500PedestalStructure:
                    return "Scp500PedestalStructure Variant";
                case PrefabType.Scp018PedestalStructure:
                    return "Scp018PedestalStructure Variant";
                case PrefabType.WorkstationStructure:
                    return "Spawnable Work Station Structure";
                case PrefabType.Scp2176PedestalStructure:
                    return "Scp2176PedestalStructure Variant";
                case PrefabType.Portal106:
                    return PlayersList.host.Hub.scp106PlayerScript.portalPrefab?.name ?? "PORTAL106";
                default:
                    return type.ToString();
            }
        }

        [Attributes.EventHandler(typeof(Events.ServerStarted))]
        internal static void Fill()
        {
            if (allPrefabs == null)
                allPrefabs = new List<Prefab>(NetworkClient.prefabs.Count);
            else
                allPrefabs.Clear();

            foreach (var prefab in NetworkClient.prefabs)
                allPrefabs.Add(new Prefab(prefab));

            if (ReferenceHub._hostHub != null)
                allPrefabs.Add(new Prefab(ReferenceHub._hostHub?.scp106PlayerScript?.portalPrefab));
        }
    }
}
