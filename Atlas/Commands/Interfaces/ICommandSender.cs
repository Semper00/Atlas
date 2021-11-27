namespace Atlas.Commands.Interfaces
{
    public interface ICommandSender
    {
        bool IsHost { get; }

        void Reply(object reply, bool isError = false);
        void ReplyConsole(object reply, string color = "green");
    }
}