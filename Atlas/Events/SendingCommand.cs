using Atlas.Entities;
using Atlas.Enums;
using Atlas.EventSystem;

using CommandSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Triggers before the <see cref="RemoteAdmin.CommandProcessor"/> processes a command.
    /// </summary>
    public class SendingCommand : BoolEvent
    {
        /// <summary>
        /// The query being processed.
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// The command's sender.
        /// </summary>
        public ICommandSender Sender { get; }

        /// <summary>
        /// The player that sent the command.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Player"/> is the host.
        /// </summary>
        public bool IsHost { get; }

        /// <summary>
        /// Gets the command's origin.
        /// </summary>
        public CommandSource Source { get; }

        public SendingCommand(string query, ICommandSender sender, CommandSource source, bool allow)
        {
            Query = query;
            Sender = sender;
            IsAllowed = allow;
            Source = source;

            Player = PlayersList.Get(sender);

            IsHost = Player == null || Player.IsHost;
        }
    }
}
