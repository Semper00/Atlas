using System.ComponentModel;

using Atlas.Interfaces;

namespace Atlas.ModManager.Configs.Atlas
{
    public class ServerConfiguration : IConfig
    {
        [Description("Turning this on will print a message to the console everytime someone gets banned.")]
        public bool LogBansToConsole { get; set; } = false;
    }
}