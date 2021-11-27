using Atlas.Commands.Enums;

namespace Atlas.Commands.Interfaces
{
    public interface IResult
    {
        CommandError? Error { get; }

        string ErrorReason { get; }

        bool IsSuccess { get; }
    }
}

