namespace Atlas.Commands
{
    public class CommandManagerConfig
    {
        public char SeparatorChar { get; set; } = ' ';

        public bool CaseSensitiveCommands { get; set; } = false;

        public bool ThrowOnError { get; set; } = true;

        public bool IgnoreExtraArgs { get; set; } = true;
    }
}
