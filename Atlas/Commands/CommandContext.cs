using System;

using Atlas.Commands.Interfaces;
using Atlas.Commands.Enums;
using Atlas.Commands.Entities;
using Atlas.Entities;

namespace Atlas.Commands
{
    public class CommandContext 
    {
        public CommandManager Manager { get; }
        public CommandModule Module { get; }
        public CommandInfo Command { get; }
        public ICommandSender Sender { get; }
        public CommandType Source { get; }

        public Player Player { get; }

        public string Text { get; }

        public CommandContext(ICommandSender sender, string text, CommandManager manager, CommandModule module, CommandType source, CommandInfo command)
        {
            Manager = manager;
            Module = module;
            Sender = sender;
            Text = text;
            Source = source;   
            Command = command; 

            if (sender is Player player)
                Player = player;
        }

        public void Reply(object reply, bool isError = false)
        {
            string str = reply.ToString();

            if (string.IsNullOrEmpty(str))
                return;

            if (Sender.IsHost)
            {
                Log.Add($"{Command.Name}#", str, isError ? ConsoleColor.Red : ConsoleColor.Green);
            }
            else
            {
                if (Source == CommandType.PlayerConsole)
                    Sender.ReplyConsole(reply, isError ? "red" : "green");
                else
                    Sender.Reply(reply, isError);
            }
        }
    }
}
