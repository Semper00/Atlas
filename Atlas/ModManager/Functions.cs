using System;
using System.IO;

using Atlas.Interfaces;
using Atlas.Events;
using Atlas.Attributes;
using Atlas.Enums;
using Atlas.EventSystem;

namespace Atlas.ModManager
{
    public static class Functions
    {
        /// <summary>
        /// Logs warning about Atlas release type.
        /// </summary>
        [EventHandler(typeof(RoundWaiting))]
        public static void LogWarnings()
        {
            if (BuildInfo.BuildType != Enums.BuildType.Release)
                Log.Warn("Atlas", $"You are running an unstable {BuildInfo.BuildType} release! This version may contain a lot of bugs!");

            if (BuildInfo.IsForBeta && BuildInfo.BetaType == BetaBranch.Staging)
                Log.Warn("Atlas", $"This version ({BuildInfo.FullVersionString}) is for a staging branch! " +
                    $"Make sure to report every issue to the Atlas development team as Northwood does NOT provide any support for plugins on this branch!");

            if (BuildInfo.IsForBeta && BuildInfo.BetaType == BetaBranch.Public)
                Log.Warn("Atlas", $"This version ({BuildInfo.FullVersionString}) is for a public beta branch! This server version does NOT represent the full" +
                    $"product and may contain a lot of bugs!");
        }

        /// <summary>
        /// Gets a new ModId instance for the specified mod.
        /// </summary>
        /// <param name="mod">The mod to identify.</param>
        /// <returns>The mod's identifier.</returns>
        public static ModId GetModId(IModBase<IConfig> mod)
            => new ModId
            {
                AssemblyLocation = ModLoader.GetPath(mod.Assembly),
                AssemblyVersion = mod.Assembly.GetName().Version.ToString(),
                AssemblyName = Path.GetFileName(ModLoader.GetPath(mod.Assembly)),

                ModName = mod.Name,
                ModVersion = mod.Version.ToString()
            };

        /// <summary>
        /// Sends a INFO level message to the console.
        /// </summary>
        /// <param name="msg">The message to send.</param>
        public static void Info(object msg)
            => Log.Info("Atlas", msg);

        /// <summary>
        /// Forces a garbage collection.
        /// </summary>
        public static void CollectGarbage()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        /// <summary>
        /// An extension that will convert an <see cref="object"/> to the <typeparam name="T">type specified as the generic parameter.</typeparam>
        /// </summary>
        /// <typeparam name="T">The type to convert to.</typeparam>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted object if their type match, otherwise null.</returns>
        public static T As<T>(this object obj)
        {
            if (obj is T tValue)
                return tValue;

            return default;
        }

        /// <summary>
        /// Handles the ServerStopping event.
        /// </summary>
        public static void HandleServerQuit()
        {
            EventManager.Invoke(new ServerStopping());

            ModLoader.Exit(false);
        }
    }
}
