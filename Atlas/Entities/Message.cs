using System.Collections.Generic;

using Atlas.Enums;

namespace Atlas.Entities
{
    /// <summary>
    /// A class used to handle broadcasts and simplify config broadcasts.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Defines how long is this message going to last. Defaults to 10 seconds.
        /// </summary>
        public ushort Duration { get; set; } = 10;

        /// <summary>
        /// Determines the text of this message. Defaults to "This is a messsage!"
        /// </summary>
        public string Text { get; set; } = "This is a message!";

        /// <summary>
        /// Determines the color of this message. Applies only to <see cref="MessageType.PlayerReport"/> and <see cref="MessageType.Console"/>. Defaults to "red"
        /// </summary>
        public string Color { get; set; } = "red";

        /// <summary>
        /// Determines the type of this message. Defaults to <see cref="MessageType.Broadcast"/>
        /// </summary>
        public MessageType Type { get; set; } = MessageType.Broadcast;

        /// <summary>
        /// Shows this message to all players.
        /// </summary>
        public void Show(bool clear = false) 
            => Show(PlayersList.List, clear);

        /// <summary>
        /// Shows this message to a specified player.
        /// </summary>
        /// <param name="player">The player.</param>
        public void Show(Player player, bool clear = false)
        {
            if (clear)
                player.Broadcaster.UserCode_TargetClearElements(player.Connection);

            if (Type == MessageType.Broadcast)
                player.Broadcaster.UserCode_TargetAddElement(player.Connection, Text, Duration, Broadcast.BroadcastFlags.Normal);

            if (Type == MessageType.BroadcastMono)
                player.Broadcaster.UserCode_TargetAddElement(player.Connection, Text, Duration, Broadcast.BroadcastFlags.Monospaced);

            if (Type == MessageType.AdminChat)
                player.Broadcaster.UserCode_TargetAddElement(player.Connection, Text, Duration, Broadcast.BroadcastFlags.AdminChat);

            if (Type == MessageType.RemoteAdmin)
                player.Hub.queryProcessor._sender.RaReply("SERVER#" + Text, true, false, "");

            if (Type == MessageType.PlayerReport)
                player.Hub.queryProcessor.GCT.SendToClient(player.Connection, Text, Color);

            if (Type == MessageType.Console)
                player.Hub.characterClassManager.TargetConsolePrint(player.Connection, Text, Color);

            if (Type == MessageType.Hint)
                player.ShowHint(Text, Duration);
        }

        /// <summary>
        /// Shows this message to specified players.
        /// </summary>
        /// <param name="players">Players list.</param>
        public void Show(IEnumerable<Player> players, bool clear = false)
        {
            foreach (Player player in players)
                Show(player, clear);
        }
    }
}