using Atlas.Entities;
using Atlas.Pools;
using Atlas.Enums;

namespace Atlas.Commands
{
    /// <summary>
    /// A class to provide context when executing commands.
    /// </summary>
    public class CommandContext : PoolableObject
    {
        internal Command cmd;
        internal Player sender;
        internal CommandSource source;
        internal CommandArgs args;

        public CommandContext() { }

        /// <summary>
        /// Gets the command that is being executed.
        /// </summary>
        public Command Command { get => cmd; }

        /// <summary>
        /// Gets the command arguments.
        /// </summary>
        public CommandArgs Args { get => args; }

        /// <summary>
        /// Gets the command sender.
        /// </summary>
        public Player Sender { get => sender; }

        /// <summary>
        /// Gets a value indicating whether or not the sender is a player.
        /// </summary>
        public bool IsPlayer { get => !sender.IsHost; }

        /// <summary>
        /// Gets a value indicating whether or not the sender is the server.
        /// </summary>
        public bool IsServer { get => sender.IsHost; }

        /// <summary>
        /// Gets an enum indicating the source of this command.
        /// </summary>
        public CommandSource Source { get => source; }

        /// <summary>
        /// Sends a reply to the player or server console.
        /// </summary>
        /// <param name="reply">The response.</param>
        /// <param name="color">The color to reply with (aplies only when sent from the player console).</param>
        public void Reply(object reply, string color = "green")
        {
            if (IsServer)
                Log.Add("COMMAND", $"{Command.Name.ToUpper()}#{reply}");
            else
                sender.SendConsoleMessage(reply.ToString(), color);
        }

        /// <summary>
        /// Sends a reply to the remote admin panel.
        /// </summary>
        /// <param name="reply">The response.</param>
        /// <param name="success">Whether or not the command was sucessfuly executed.</param>
        public void RaReply(object reply, bool success = true)
        {
            if (IsServer)
                Log.Add("COMMAND", $"{Command.Name.ToUpper()}#{reply}");
            else
                sender.Sender.Respond(reply.ToString(), success);
        }

        public override void OnReturned()
        {
            cmd = null;
            sender = null;
            args = null;
        }
    }
}
