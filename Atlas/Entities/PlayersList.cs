using System.Collections.Generic;
using System.Linq;
using System;

using UnityEngine;

using Atlas.Extensions;
using Atlas.Exceptions;

using CommandSystem;
using RemoteAdmin;

namespace Atlas.Entities
{
    /// <summary>
    /// A class used for player management.
    /// </summary>
    public static class PlayersList
    {
        internal static Dictionary<ReferenceHub, Player> players;
        internal static List<Player> dummyPlayers;

        internal static Player host;

        static PlayersList()
        {
            players = new Dictionary<ReferenceHub, Player>();
            dummyPlayers = new List<Player>();
        }

        /// <summary>
        /// Gets the hosting player / server player.
        /// </summary>
        public static Player Host => host;

        /// <summary>
        /// Gets a read-only collection of all players on the server.
        /// </summary>
        public static IReadOnlyCollection<Player> List => players.Values;

        /// <summary>
        /// Gets a read-only list of all spawned dummies by the <see cref="Dummy"/> class.
        /// </summary>
        public static IReadOnlyList<Player> DummyPlayers => dummyPlayers;

        /// <summary>
        /// Gets a read-only dictionary of all players and their <see cref="ReferenceHub"/>s.
        /// </summary>
        public static IReadOnlyDictionary<ReferenceHub, Player> Dictionary => players;

        /// <summary>
        /// Gets a <see cref="HashSet{T}"/> of players that can't trigger SCP-096.
        /// </summary>
        public static HashSet<Player> Scp096TurnedPlayers { get; } = new HashSet<Player>();

        /// <summary>
        /// Gets a <see cref="HashSet{T}"/> of players that can't stop SCP-173.
        /// </summary>
        public static HashSet<Player> Scp173TurnedPlayers { get; } = new HashSet<Player>();

        /// <summary>
        /// Gets the amount of all players on the server, if any.
        /// </summary>
        public static int Count => List.Count;

        /// <summary>
        /// Adds a player to the list.
        /// </summary>
        /// <param name="hub">The <see cref="ReferenceHub"/> of the player.</param>
        /// <returns>The player.</returns>
        public static Player Create(ReferenceHub hub)
        {
            if (hub == null)
                throw new HubNotFoundException("hub");

            if (players.TryGetValue(hub, out Player player))
                return player;

            player = new Player(hub);

            players.Add(hub, player);

            return player;
        }

        /// <summary>
        /// Clears the player list. Not recommended to use as it can <b>REALLY</b> mess up the API. <b>Only used for round restarts.</b>
        /// </summary>
        public static void Clear() 
            => players.Clear();

        /// <summary>
        /// Gets a list of players by their <see cref="Team"/>.
        /// </summary>
        /// <param name="team">The team to filter by.</param>
        /// <returns>A list of players matching this team.</returns>
        public static IEnumerable<Player> Get(Team team) 
            => Get(x => x.Team == team);

        /// <summary>
        /// Gets a list of all players by their <see cref="RoleType"/>.
        /// </summary>
        /// <param name="role">The role to filter by.</param>
        /// <returns>A list of players matching this role.</returns>
        public static IEnumerable<Player> Get(RoleType role) 
            => Get(x => x.Role == role);

        /// <summary>
        /// Gets a list of all players by their <see cref="Fraction"/>.
        /// </summary>
        /// <param name="role">The fraction to filter by.</param>
        /// <returns>A list of players matching this fraction.</returns>
        public static IEnumerable<Player> Get(Faction fraction) 
            => Get(x => x.Fraction == fraction);

        /// <summary>
        /// Gets a list of all players by this predicate.
        /// </summary>
        /// <param name="predicate">The predicate to search by.</param>
        /// <returns>A list of all matching players.</returns>
        public static IEnumerable<Player> Get(Func<Player, bool> predicate) 
            => players.Values.Where(predicate);

        /// <summary>
        /// Counts players that meet the predicate.
        /// </summary>
        /// <param name="predicate">The predicate to sort by.</param>
        /// <returns>The amount of players that met the predicate.</returns>
        public static int GetCount(Func<Player, bool> predicate)
            => players.Values.Count(predicate);

        /// <summary>
        /// Gets a player by his <see cref="ReferenceHub"/>.
        /// </summary>
        /// <param name="hub">The <see cref="ReferenceHub"/> to search by.</param>
        /// <returns>The player found, null if not.</returns>
        public static Player Get(ReferenceHub hub) 
            => players.TryGetValue(hub, out Player player) ? player : null;

        /// <summary>
        /// Gets a player by his <see cref="GameObject"/>.
        /// </summary>
        /// <param name="gameObject">The <see cref="GameObject"/> to search by.</param>
        /// <returns>The player found, null if not.</returns>
        public static Player Get(GameObject gameObject)
        {
            if (ReferenceHub.TryGetHub(gameObject, out ReferenceHub hub))
                return Get(hub);

            return null;
        }

        /// <summary>
        /// Gets a player by his network identity.
        /// </summary>
        /// <param name="netId">The ID to search by.</param>
        /// <returns>The player found, null if not.</returns>
        public static Player Get(uint netId)
        {
            if (ReferenceHub.TryGetHubNetID(netId, out ReferenceHub hub))
                return Get(hub);

            return null;
        }

        /// <summary>
        /// Gets a player from his <see cref="CommandSender"/>.
        /// </summary>
        /// <param name="sender">The player to get.</param>
        /// <returns>The player instance if found, otherwise null.</returns>
        public static Player Get(CommandSender sender)
        {
            if (sender is PlayerCommandSender pcs)
                return Get(pcs.ReferenceHub);
            else
                return Get(sender.SenderId);
        }

        /// <summary>
        /// Gets a player from his <see cref="ICommandSender"/>.
        /// </summary>
        /// <param name="sender">The player to get.</param>
        /// <returns>The player instance if found, otherwise null.</returns>
        public static Player Get(ICommandSender sender)
        {
            if (sender is CommandSender s)
                return Get(s);

            return null;
        }

        /// <summary>
        /// Gets a player by his Player ID.
        /// </summary>
        /// <param name="playerId">The player ID to search by.</param>
        /// <returns>The player found, null if not.</returns>
        public static Player Get(int playerId)
        {
            if (ReferenceHub.TryGetHub(playerId, out ReferenceHub hub))
                return Get(hub);

            return null;
        }

        /// <summary>
        /// Gets a player by his User ID.
        /// </summary>
        /// <param name="userId">The UserID to search by.</param>
        /// <returns>The player found, null if not.</returns>
        public static Player GetById(string userId)
        {
            foreach (KeyValuePair<ReferenceHub, Player> player in players)
            {
                if (player.Value.UserId == userId)
                    return player.Value;

                if (player.Value.CustomUserId == userId)
                    return player.Value;
            }

            return null;
        }

        /// <summary>
        /// Gets a player by his name or UserID.
        /// </summary>
        /// <param name="query">The query to search by.</param>
        /// <returns>Whether or not the player was found.</returns>
        public static bool TryGet(string query, out Player player)
        {
            player = Get(query);

            return player != null;
        }

        /// <summary>
        /// Gets a player by his name or UserID.
        /// </summary>
        /// <param name="query">The query to search by.</param>
        /// <returns>The player found, null if not.</returns>
        public static Player Get(string query)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query))
                    return null;

                if (int.TryParse(query, out int id))
                    return Get(id);

                Player playerFound = null;

                if (query.EndsWith("@steam") || query.EndsWith("@discord") || query.EndsWith("@northwood") || query.EndsWith("@patreon"))
                {
                    foreach (Player player in players.Values)
                    {
                        if (player.UserId == query)
                        {
                            playerFound = player;

                            break;
                        }
                    }
                }
                else
                {
                    int maxNameLength = 31; 
                    int lastnameDifference = 31;

                    string firstString = query.ToLower();

                    foreach (Player player in players.Values)
                    {
                        if (!player.IsVerified || player.Nickname == null)
                            continue;

                        if (!player.Nickname.Contains(query))
                            continue;

                        if (firstString.Length < maxNameLength)
                        {
                            int x = maxNameLength - firstString.Length;
                            int y = maxNameLength - player.Nickname.Length;

                            string secondString = player.Nickname;

                            for (int i = 0; i < x; i++)
                                firstString += "z";

                            for (int i = 0; i < y; i++)
                                secondString += "z";

                            int nameDifference = firstString.GetDistance(secondString);

                            if (nameDifference < lastnameDifference)
                            {
                                lastnameDifference = nameDifference;

                                playerFound = player;
                            }
                        }
                    }
                }

                return playerFound;
            }
            catch 
            {
                return null;
            }
        }
    }
}