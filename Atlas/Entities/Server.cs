using GameCore;

using System.Diagnostics;

using Atlas.Enums;

namespace Atlas.Entities
{
    /// <summary>
    /// A class for server management.
    /// </summary>
    public static class Server
    {
        static Server()
        {
            Version = new System.Version(GameCore.Version.Major, GameCore.Version.Minor, GameCore.Version.Revision);
            Random = new System.Random();
        }

        /// <summary>
        /// Gets or sets the server's name.
        /// </summary>
        public static string Name
        {
            get => ServerConsole._serverName;
            set
            {
                ServerConsole._serverName = value;
                ServerConsole.ReloadServerName();
            }
        }

        /// <summary>
        /// Gets the server's IP address.
        /// </summary>
        public static string IpAddress { get => ServerConsole.Ip; }

        /// <summary>
        /// Gets the server's version.
        /// </summary>
        public static System.Version Version { get; }

        /// <summary>
        /// Gets the <see cref="System.Random"/> generator instance.
        /// </summary>
        public static System.Random Random { get; }

        /// <summary>
        /// Gets the server's <see cref="global::PermissionsHandler"/>.
        /// </summary>
        public static PermissionsHandler PermissionsHandler { get => ServerStatic.PermissionsHandler; }

        /// <summary>
        /// Gets the local player.
        /// </summary>
        public static Player Host { get => PlayersList.host; }

        /// <summary>
        /// Gets the server's <see cref="System.Diagnostics.Process"/>.
        /// </summary>
        public static Process Process { get => Process.GetCurrentProcess(); }

        /// <summary>
        /// Gets or sets the <see cref="NextAction"/> to be performed when the round ends.
        /// </summary>
        public static NextAction RoundAction { get => (NextAction)ServerStatic.StopNextRound; set => ServerStatic.StopNextRound = (ServerStatic.NextRoundAction)value; }

        /// <summary>
        /// Gets the server's port.
        /// </summary>
        public static ushort Port { get => ServerStatic.ServerPortSet ? ServerStatic.ServerPort : (ushort)7777; }

        /// <summary>
        /// Gets or sets the server's tickrate.
        /// </summary>
        public static short Tickrate { get => ServerStatic.ServerTickrate; set => ServerStatic.ServerTickrate = value; }

        /// <summary>
        /// Gets the number of players.
        /// </summary>
        public static int Players { get => PlayersList.Count; }

        /// <summary>
        /// Gets or sets the maximum number of players.
        /// </summary>
        public static int MaxPlayers
        {
            get => CustomNetworkManager.TypedSingleton.ReservedMaxPlayers;
            set
            {
                ConfigFile.ServerConfig.SetString("max_players", value.ToString());

                ConfigFile.ReloadGameConfigs();
            }
        }

        /// <summary>
        /// Reloads the game configs, plugins and remote admin.
        /// </summary>
        public static void Reload()
        {
            ConfigFile.ReloadGameConfigs();

            ModManager.ModLoader.Reload();

            PermissionsHandler.RefreshPermissions();
        }

        /// <summary>
        /// Safely restarts the server.
        /// </summary>
        public static void Restart()
        {
            var stats = Host?.Hub.playerStats ?? PlayerManager.localPlayer?.GetComponent<PlayerStats>();

            stats?.RpcRoundrestart(0f, false);
            RoundAction = NextAction.Restart;
            stats.Roundrestart();
        }

        /// <summary>
        /// Kills the server process, resulting in a shutdown.
        /// </summary>
        public static void Kill()
            => Shutdown.Quit(true);
    }
}
