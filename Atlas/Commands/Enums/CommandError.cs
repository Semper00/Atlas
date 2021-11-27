namespace Atlas.Commands.Enums
{
    public enum CommandError
    {
        UnknownCommand = 1,
        ParseFailed,
        BadArgCount,
        ObjectNotFound,
        MultipleMatches,
        UnmetPrecondition,
        Exception,
        Unsuccessful
    }
}
