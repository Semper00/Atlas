using Atlas.Commands.Interfaces;
using Atlas.Commands.Entities;
using Atlas.Commands.Enums;
using Atlas.Entities;

namespace Atlas.Commands
{
    public abstract class CommandModule : ICommandModule
    {
        public CommandManager Manager { get => Context?.Manager; }
        public CommandModule Module { get => Context?.Module; }
        public ICommandSender Sender { get => Context?.Sender; }
        public CommandType Source { get => Context?.Source ?? CommandType.ServerConsole; }
        public CommandInfo Command { get => Context?.Command; }
        public CommandContext Context { get; private set; }

        public Player Player { get => Context?.Player; }

        public void Reply(object reply, bool isError = false)
        {
            Context?.Reply(reply, isError);
        }

        void ICommandModule.SetContext(CommandContext context)
        {
            Context = context;
        }
    }
}