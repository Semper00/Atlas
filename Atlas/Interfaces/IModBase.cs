using System;
using System.Reflection;
using System.Collections.Generic;

using Atlas.Translations;

namespace Atlas.Interfaces
{
    /// <summary>
    /// An interface used as a base of <see cref="Mod.ModBase{TConfig}"/>
    /// </summary>
    public interface IModBase<out TConfig> where TConfig : IConfig
    {
        string Author { get; }
        string Name { get; }
        string Description { get; }

        TConfig Config { get; }

        Dictionary<string, Translation> Translations { get; }

        Type PluginClass { get; }
        Assembly Assembly { get; }
        Version Version { get; }
        Version Required { get; }

        void Enable();
        void Disable();
        void Reload();

        void RegisterEvents<T>(T instance);
        void RegisterCommands<T>();
        void UnregisterCommmands<T>();
        void UnregisterEvents<T>();
        void UnregisterEvents();
        void UnregisterCommands();
        void SaveConfig();
        void ReloadConfig();
    }
}