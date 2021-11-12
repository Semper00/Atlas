using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using Atlas.Interfaces;
using Atlas.Enums;
using Atlas.Commands.Attributes;

namespace Atlas.Commands
{
    /// <summary>
    /// A nice little <see cref="CommandAttribute"/> wrapper to simplify command management.
    /// </summary>
    public class Command
    {
        /// <summary>
        /// Gets the owner of this command.
        /// </summary>
        public CommandAttribute Parent { get; }

        /// <summary>
        /// Gets the method to execute.
        /// </summary>
        public MethodInfo Method { get; }

        /// <summary>
        /// Gets assigned argument converters.
        /// </summary>
        public Dictionary<int, IConverter> AssignedArgConverters { get; }

        /// <summary>
        /// Gets method overload parameters.
        /// </summary>
        public ParameterInfo[] Parameters { get; }

        /// <summary>
        /// Gets the object the method is going to be invoked on.
        /// </summary>
        public object Invoker { get; }

        /// <summary>
        /// Gets the command name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the command description.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets the command usage.
        /// </summary>
        public string Usage { get; }

        /// <summary>
        /// Gets the command environments.
        /// </summary>
        public HashSet<CommandSource> Environment { get; }

        /// <summary>
        /// Gets the command aliases.
        /// </summary>
        public string[] Aliases { get; }

        /// <summary>
        /// Gets a value indicating whether or not to convert arguments.
        /// </summary>
        public bool ConvertArguments { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class.
        /// </summary>
        /// <param name="parent">The parent attribute.</param>
        /// <param name="method">The method to execute.</param>
        /// <param name="parameters">Method overload parameters.</param>
        /// <param name="ownerInstance">Instance of the owner type.</param>
        public Command(CommandAttribute parent, MethodInfo method, ParameterInfo[] parameters, object invoker)
        {
            Parent = parent;
            Invoker = invoker;
            AssignedArgConverters = new Dictionary<int, IConverter>();

            Name = parent.nameSupplied ? parent.Command : method.Name;
            Aliases = parent.nameSupplied ? parent.Aliases : null;

            Description = method.GetCustomAttribute<CommandDescriptionAttribute>()?.Description ?? "";
            Usage = method.GetCustomAttribute<CommandUsageAttribute>()?.Usage ?? Name + "" + string.Join(" ", parameters.Where(x => x.ParameterType != typeof(CommandContext)).Select(x => x.Name));
            Environment = method.GetCustomAttribute<CommandEnvironmentAttribute>()?.Environments ?? new HashSet<CommandSource>() { CommandSource.RemoteAdmin, CommandSource.ServerConsole };

            ConvertArguments = parameters.Length >= 2;

            Parameters = parameters;
            Method = method;

            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].ParameterType == typeof(CommandContext))
                    continue;

                if (CommandManager.GetConverters().TryGetValue(parameters[i].ParameterType, out IConverter converter))
                {
                    if (!AssignedArgConverters.ContainsKey(i))
                        AssignedArgConverters.Add(i, converter);
                }
            }

            Log.Debug("Atlas", $"Command Handler created: {method.DeclaringType.FullName}.{method.Name}");
        }

        ~Command()
        {
            AssignedArgConverters.Clear();
        }

        /// <summary>
        /// Executes a command with the given <see cref="CommandContext"/>.
        /// </summary>
        /// <param name="ctx">The <see cref="CommandContext"/> to execute this command with.</param>
        public void ExecuteCommand(CommandContext ctx)
        {
            if (ConvertArguments)
            {
                if (ctx.Args.Count < Parameters.Length)
                {
                    ctx.RaReply($"Missing arguments!\nUsage: {Usage}", false);

                    return;
                }

                object[] parameters = new object[Parameters.Length];

                parameters[0] = ctx;

                for (int i = 0; i < Parameters.Length; i++)
                {
                    parameters[i] = AssignedArgConverters.TryGetValue(i, out IConverter converter) ? converter.Convert(ctx.Args.Get(i)) : null;
                }

                Method.Invoke(Invoker, parameters);
            }
            else
            {
                Method.Invoke(Invoker, new object[] { ctx });
            }
        }
    }
}