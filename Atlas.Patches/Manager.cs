using System;
using System.Reflection;
using System.Collections.Generic;

using NorthwoodLib.Pools;

using HarmonyLib;

using Atlas;

namespace Atlas.Patches
{
    /// <summary>
    /// This class manages all patches.
    /// </summary>
    public class Manager
    {
        internal Assembly assembly;

        internal List<Type> dis;
        internal List<MethodBase> patches;

        internal Dictionary<Type, Type> linkedPatches;

        internal Harmony harmony;

        internal bool patched;
        internal int indexer;

        public Manager()
        {
            assembly = typeof(Manager).Assembly;

            dis = ListPool<Type>.Shared.Rent();
            patches = ListPool<MethodBase>.Shared.Rent();

            linkedPatches = new Dictionary<Type, Type>();

            harmony = new Harmony($"Atlas@{indexer++}");

            var eventAssembly = typeof(BuildInfo).Assembly;

            foreach (Type type in assembly.GetTypes())
            {
                if (type.Namespace != "Atlas.Patches.Events")
                    continue;

                string eventName = type.Name.Replace("Patch", "");

                Type eventType = eventAssembly.GetType($"Atlas.Events.{eventName}");

                if (eventType != null)
                {
                    linkedPatches.Add(type, eventType);

                    Log.DebugFeature<Manager>($"Linked {type.FullName} to {eventType.FullName}");
                }
            }

            Log.Info("Atlas", $"Linked {linkedPatches.Count} patches!");
        }

        ~Manager()
        {
            ListPool<Type>.Shared.Return(dis);
            ListPool<MethodBase>.Shared.Return(patches);

            linkedPatches.Clear();

            harmony = null;
        }

        internal static void Exc<T>(Exception e)
        {
            string name = typeof(T).FullName;

            Log.Error("Atlas", $"There has been an exception while executing the {name} patch!");
            Log.Error("Atlas", e);
        }

        /// <summary>
        /// Gets a <see cref="IReadOnlyList{T}"/> of disabled patches.
        /// </summary>
        public IReadOnlyList<Type> DisabledPatches { get => dis; }

        /// <summary>
        /// Gets a <see cref="IReadOnlyList{T}"/> of patched methods.
        /// </summary>
        public IReadOnlyList<MethodBase> Patches { get => patches; }

        /// <summary>
        /// Gets a <see cref="IReadOnlyDictionary{TKey, TValue}"/> of linked patches.
        /// </summary>
        public IReadOnlyDictionary<Type, Type> LinkedPatches { get => linkedPatches; }

        /// <summary>
        /// Gets a value indicating whether or not to show debug messages.
        /// </summary>
        public bool IsDebug => BuildInfo.BuildType != Enums.BuildType.Release;

        /// <summary>
        /// Gets a value indicating whether the manager has patched all patches.
        /// </summary>
        public bool IsPatched => patched;

        /// <summary>
        /// Disables a previously enabled patch.
        /// </summary>
        /// <param name="type">The patch to disable.</param>
        public void DisablePatch(Type type)
        {
            Debug($"Attempting to disable the {type.FullName} patch.");

            if (!dis.Contains(type))
                dis.Add(type);

            ReloadPatches();
        }

        /// <summary>
        /// Enables a previously disabled patch.
        /// </summary>
        /// <param name="type">The patch to enable.</param>
        public void EnablePatch(Type type)
        {
            Debug($"Attempting to enable the {type.FullName} patch ...");

            if (dis.Remove(type))
                ReloadPatches();
        }

        internal void Info(object msg)
            => Log.Info("Atlas", msg);

        internal void Debug(object msg)
        {
            if (!IsDebug)
                return;

            Log.Debug("Atlas", msg);
        }

        /// <summary>
        /// Unpatches all patches.
        /// </summary>
        public void UnpatchPatches()
        {
            if (patched)
            {
                Info("Unpatching ..");

                foreach (MethodBase patch in patches)
                {
                    Debug($"Unpatching {patch.DeclaringType}.{patch.Name} ..");

                    harmony.Unpatch(patch, HarmonyPatchType.All, harmony.Id);
                }
            }

            patched = false;
        }

        /// <summary>
        /// Patches all patches.
        /// </summary>
        public void PatchPatches()
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (!type.IsPublic || !type.FullName.EndsWith("Patch"))
                    continue;

                if (dis.Contains(type))
                    continue;

                try
                {
                    _ = harmony.CreateClassProcessor(type)?.Patch();

                    Debug($"Patched {type.FullName}.");
                }
                catch (Exception e)
                {
                    Info(e);

                    continue;
                }
            }

            patches.Clear();
            patches.AddRange(harmony.GetPatchedMethods());
            patched = true;

            Info("Patching completed!");
        }

        /// <summary>
        /// Reloads all patches.
        /// </summary>
        public void ReloadPatches()
        {
            try
            {
                Info("Reloading patches ...");

                UnpatchPatches();

                PatchPatches();
            }
            catch (Exception e)
            {
                Info(e);
            }
        }
    }
}
