using System.Collections.Generic;

using Atlas.Commands.Converters.Results;
using Atlas.Commands.Interfaces;

namespace Atlas.Commands.Entities
{
    public struct CommandMatch
    {
        public CommandInfo Command { get; }

        public string Alias { get; }

        public CommandMatch(CommandInfo command, string alias)
        {
            Command = command;
            Alias = alias;
        }

        public ParseResult Parse(CommandContext context, SearchResult searchResult)
            => Command.Parse(context, Alias.Length, searchResult);

        public IResult Execute(CommandContext context, IEnumerable<object> argList, IEnumerable<object> paramList)
            => Command.Execute(context, argList, paramList);

        public IResult Execute(CommandContext context, ParseResult parseResult)
            => Command.Execute(context, parseResult);
    }
}
