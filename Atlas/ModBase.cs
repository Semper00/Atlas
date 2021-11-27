using System;
using System.Linq;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;

using Atlas.ModManager;
using Atlas.Interfaces;
using Atlas.Commands;
using Atlas.Commands.Entities;
using Atlas.Translations;
using Atlas.EventSystem;

namespace Atlas
{
    /// <summary>
    /// A class used to manage mods.
    /// </summary>
    /// <typeparam name="TConfig">A reference to your config class.</typeparam>
    public class ModBase<TConfig> : IModBase<TConfig> where TConfig : IConfig, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModBase{TConfig}"/> class.
        /// </summary>
        public ModBase()
        {
            PluginClass = GetType();

            Assembly = Assembly.GetAssembly(PluginClass);

            Path = ModLoader.GetPath(Assembly);

            AssemblyName name = Assembly.GetName();

            string author = FileVersionInfo.GetVersionInfo(Path).CompanyName;

            Author = string.IsNullOrEmpty(author) ? name.Name : author;

            Name = name.Name;

            Version = name.Version;

            Description = "";

            Config = new TConfig();
            Translations = new Dictionary<string, Translation>();

            Required = BuildInfo.Version;
        }

        ~ModBase()
        {

        }

        /// <summary>
        /// Gets the mod's class.
        /// </summary>
        public Type PluginClass { get; }

        /// <summary>
        /// Gets or sets the plugin's command manager.
        /// </summary>
        public CommandManager Manager { get; private set; }

        /// <summary>
        /// Gets the mod's author.
        /// </summary>
        public virtual string Author { get; }

        /// <summary>
        /// Gets the mod's name.
        /// </summary>
        public virtual string Name { get; }

        /// <summary>
        /// Gets the mod's description.
        /// </summary>
        public virtual string Description { get; }

        /// <summary>
        /// Gets the mod's config.
        /// </summary>
        public virtual TConfig Config { get; }

        /// <summary>
        /// Get's the mod's <see cref="System.Version"/>
        /// </summary>
        public virtual Version Version { get; }

        /// <summary>
        /// Gets the mod's required <see cref="System.Version"/>
        /// </summary>
        public virtual Version Required { get; }

        /// <summary>
        /// Gets the assembly of this mod.
        /// </summary>
        public Assembly Assembly { get; }

        /// <summary>
        /// Gets the path to the assembly of this mod.
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// Gets all commands this mod has registered.
        /// </summary>
        public IEnumerable<CommandInfo> Commands
            => Manager.Commands;

        /// <summary>
        /// Gets a list of all <see cref="EventHandler"/>s registered in this mod.
        /// </summary>
        public HashSet<EventSystem.EventHandler> EventHandlers
            => EventManager.GetHandlers(Assembly);
        
        /// <summary>
        /// Gets all the mod translations.
        /// </summary>
        public virtual Dictionary<string, Translation> Translations { get; }

        /// <summary>
        /// Adds an informational message to the server output.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Info(object message)
            => Log.Info(Name, message);

        /// <summary>
        /// Adds a debug message to the server output.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Debug(object message)
            => Log.Debug(Name, message);

        /// <summary>
        /// Adds a warning message to the server output.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Warn(object message)
            => Log.Warn(Name, message);

        /// <summary>
        /// Adds an error message to the server output.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Error(object message)
            => Log.Error(Name, message);

        /// <summary>
        /// Fires when the mod gets enabled.
        /// </summary>
        public virtual void Enable()
            => Info("Succesfully enabled.");

        /// <summary>
        /// Fires when the mod gets disabled.
        /// </summary>
        public virtual void Disable()
            => Info("Disabled.");

        /// <summary>
        /// Fires when the mod gets reloaded.
        /// </summary>
        public virtual void Reload()
            => Info("Reloaded.");

        /// <summary>
        /// Registers all events inside a class.
        /// </summary>
        /// <typeparam name="T">The class to register from.</typeparam>
        /// <param name="instance">The instance of the class - <b>required unless the class is static.</b></param>
        public void RegisterEvents<T>(T instance = default)
            => EventManager.Register(instance);

        /// <summary>
        /// Registers all commands inside a module.
        /// </summary>
        /// <typeparam name="T">The module's type.</typeparam>
        public void RegisterCommands<T>()
            => RegisterCommands<T>(null);

        /// <summary>
        /// Registers all commands inside a module.
        /// </summary>
        /// <typeparam name="T">The module's type.</typeparam>
        /// <param name="config">The config for your CommandManager instance.</param>
        public void RegisterCommands<T>(CommandManagerConfig config = null)
        {
            if (Manager == null)
                Manager = new CommandManager(config == null ? new CommandManagerConfig() : config);

            Manager.AddModule<T>();
        }

        /// <summary>
        /// Unregisters all commands inside a class.
        /// </summary>
        /// <typeparam name="T">The class to unregister commands in.</typeparam>
        public void UnregisterCommmands<T>()
        {
            if (Manager == null)
                return;

            Manager.RemoveModule<T>();
        }

        /// <summary>
        /// Unregisters all events inside a class.
        /// </summary>
        /// <typeparam name="T">The class to unregister events in.</typeparam>
        public void UnregisterEvents<T>()
            => EventManager.Unregister<T>();

        /// <summary>
        /// Unregisters all events.
        /// </summary>
        public void UnregisterEvents()
            => EventManager.Unregister(Assembly);

        /// <summary>
        /// Unregisters all commands.
        /// </summary>
        public void UnregisterCommands()
        {
            if (Manager == null)
                return;

            Manager.Commands.ToList().ForEach(x => Manager.RemoveModule(x.Module));
        }

        /// <summary>
        /// Saves the mod's config file with all current values.
        /// </summary>
        public void SaveConfig()
            => ConfigManager.Save(this as IModBase<IConfig>);

        /// <summary>
        /// Reads the mod's config file and tries to set those values to the mod's config.
        /// </summary>
        public void ReloadConfig()
            => ConfigManager.Reload(this as IModBase<IConfig>);
    }
}