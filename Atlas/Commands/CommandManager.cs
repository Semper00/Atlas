using System;
using System.Reflection;
using System.Collections.Generic;

using Atlas.Interfaces;
using Atlas.Entities;
using Atlas.Pools;
using Atlas.Enums;
using Atlas.Commands.Attributes;
using Atlas.Commands.Converters;

namespace Atlas.Commands
{
    /// <summary>
    /// A static class used to manage commands.
    /// </summary>
    public class CommandManager
    {
        private static Dictionary<Type, IConverter> _convs;
        private static Dictionary<Type, HashSet<Command>> _cmds;

        static CommandManager()
        {
            _cmds = new Dictionary<Type, HashSet<Command>>();

            _convs = new Dictionary<Type, IConverter>()
            {
                [typeof(int)] = new IntegerConverter(),
                [typeof(Player)] = new PlayerConverter()
            };
        }

        /// <summary>
        /// Executes a command from the given command line.
        /// </summary>
        /// <param name="source">The source of the command.</param>
        /// <param name="sender">The player who sent the command.</param>
        /// <param name="line">The command line.</param>
        /// <returns>true if the command was executed succesfully, otherwise false.</returns>
        public static bool ExecuteCommand(CommandSource source, Player sender, string line)
        {
            string[] args = line.Split(' ');

            if (args.Length < 1)
                return false;

            CommandContext ctx = null;
            Command cmd = null;

            string cmdName = args[0];

            foreach (HashSet<Command> set in _cmds.Values)
            {
                foreach (Command command in set)
                {
                    if (!command.Environment.Contains(source))
                        continue;

                    if (command.Name == cmdName)
                    {
                        ctx = ObjectPool<CommandContext>.Shared.Rent();
                        cmd = command;

                        ctx.args = ObjectPool<CommandArgs>.Shared.Rent();
                        ctx.cmd = cmd;
                        ctx.sender = sender;
                        ctx.source = source;

                        break;
                    }

                    if (command.Aliases.Contains(cmdName))
                    {
                        ctx = ObjectPool<CommandContext>.Shared.Rent();
                        cmd = command;

                        ctx.args = ObjectPool<CommandArgs>.Shared.Rent();
                        ctx.cmd = cmd;
                        ctx.sender = sender;
                        ctx.source = source;

                        break;
                    }
                }
            }

            if (ctx == null || cmd == null)
                return false;

            try
            {
                cmd.ExecuteCommand(ctx);

                ObjectPool<CommandArgs>.Shared.Return(ctx.args);
                ObjectPool<CommandContext>.Shared.Return(ctx);

                return true;
            }
            catch (Exception e)
            {
                Log.Add("Atlas", $"An error occured while trying to execute the {cmd.Name} command! ({cmd.Method.DeclaringType.FullName}.{cmd.Method.Name})");
                Log.Add("Atlas", e);

                return false;
            }
        }

        /// <summary>
        /// Registers all commands from this class.
        /// </summary>
        /// <typeparam name="T">The class.</typeparam>
        public static void RegisterCommands<T>(T instance) 
            => RegisterCommands(typeof(T), instance);

        /// <summary>
        /// Registers all commands from this class.
        /// </summary>
        /// <param name="type">The class.</param>
        public static void RegisterCommands(Type type, object instance)
        {
            CommandAttribute typeAttr = type.GetCustomAttribute<CommandAttribute>();

            if (typeAttr != null)
            {
                if (!_cmds.ContainsKey(type))
                    _cmds.Add(type, new HashSet<Command>());

                typeAttr.owner = type;

                foreach (MethodInfo info in type.GetMethods())
                {
                    if (!info.IsPublic)
                        continue;

                    ParameterInfo[] param = info.GetParameters();

                    if (param == null || param.Length < 1)
                        continue;

                    if (param[0].ParameterType != typeof(CommandContext))
                        continue;

                    Command command = new Command(typeAttr, info, param, instance);

                    _cmds[type].Add(command);
                }
            }
            else
            {
                if (!_cmds.ContainsKey(type))
                    _cmds.Add(type, new HashSet<Command>());

                foreach (MethodInfo info in type.GetMethods())
                {
                    if (!info.IsPublic)
                        continue;

                    CommandAttribute methodAttr = info.GetCustomAttribute<CommandAttribute>();

                    if (methodAttr == null)
                        continue;

                    ParameterInfo[] param = info.GetParameters();

                    if (param == null || param.Length < 1)
                        continue;

                    if (param[0].ParameterType != typeof(CommandContext))
                        continue;

                    methodAttr.owner = type;

                    Command command = new Command(methodAttr, info, param, instance);

                    _cmds[type].Add(command);
                }
            }
        }

        /// <summary>
        /// Registers all commands in an assembly.
        /// </summary>
        /// <param name="assembly"></param>
        public static void RegisterCommands(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
                RegisterCommands(type, null);
        }

        /// <summary>
        /// Unregisters all commmands in this class.
        /// </summary>
        /// <typeparam name="T">The class.</typeparam>
        public static void UnregisterCommands<T>() 
            => UnregisterCommands(typeof(T));

        /// <summary>
        /// Unregisters all commands in this class.
        /// </summary>
        /// <param name="type">The class.</param>
        public static void UnregisterCommands(Type type)
        {
            if (_cmds.ContainsKey(type))
            {
                _cmds[type].Clear();

                _cmds.Remove(type);
            }
        }

        /// <summary>
        /// Unregisters all commands in an assembly.
        /// </summary>
        /// <param name="assembly"></param>
        public static void UnregisterCommands(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
                UnregisterCommands(type);
        }

        /// <summary>
        /// Gets all commands in an assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static HashSet<Command> GetCommands(Assembly assembly)
        {
            HashSet<Command> set = new HashSet<Command>();

            foreach (KeyValuePair<Type, HashSet<Command>> pair in _cmds)
            {
                if (assembly.GetTypes().Contains(pair.Key))
                {
                    set.UnionWith(pair.Value);
                }
            }

            return set;
        }

        /// <summary>
        /// Gets the <see cref="IConverter"/> for this class or null if none were found.
        /// </summary>
        /// <typeparam name="T">The class you want a converter instance of.</typeparam>
        /// <returns>The converter instance.</returns>
        public static IConverter GetConverter<T>() 
            => _convs.TryGetValue(typeof(T), out IConverter conv) ? conv : null;

        /// <summary>
        /// Gets all registered converters.
        /// </summary>
        /// <returns></returns>
        public static IReadOnlyDictionary<Type, IConverter> GetConverters() 
            => _convs;

        /// <summary>
        /// Register a new type converter.
        /// </summary>
        /// <typeparam name="T">The class this converter converts to.</typeparam>
        /// <param name="converter">The converter instance.</param>
        public static void RegisterConverter<T>(IConverter converter) where T : new()
        {
            Type type = typeof(T);

            if (_convs.ContainsKey(type))
                throw new Exception("A converter for that type already exists!");

            _convs.Add(type, converter);
        }

        /// <summary>
        /// Unregisters all converters for this class.
        /// </summary>
        /// <typeparam name="T">The class.</typeparam>
        public static bool UnregisterConverter<T>() where T : IConverter
            => _convs.Remove(typeof(T));
    }
}