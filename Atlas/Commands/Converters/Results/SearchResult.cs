using System;
using System.Collections.Generic;

using Atlas.Commands.Entities;
using Atlas.Commands.Interfaces;
using Atlas.Commands.Enums;

namespace Atlas.Commands.Converters.Results
{
    public struct SearchResult : IResult
    {
        public string Text { get; }
        public IReadOnlyList<CommandMatch> Commands { get; }

        public CommandError? Error { get; }

        public string ErrorReason { get; }

        public bool IsSuccess => !Error.HasValue;

        private SearchResult(string text, IReadOnlyList<CommandMatch> commands, CommandError? error, string errorReason)
        {
            Text = text;
            Commands = commands;
            Error = error;
            ErrorReason = errorReason;
        }

        public static SearchResult FromSuccess(string text, IReadOnlyList<CommandMatch> commands)
            => new SearchResult(text, commands, null, null);

        public static SearchResult FromError(CommandError error, string reason)
            => new SearchResult(null, null, error, reason);

        public static SearchResult FromError(Exception ex)
            => FromError(CommandError.Exception, ex.Message);

        public static SearchResult FromError(IResult result)
            => new SearchResult(null, null, result.Error, result.ErrorReason);

        public override string ToString() => IsSuccess ? "Success" : $"{Error}: {ErrorReason}";
    }
}
