using System.Collections.Generic;

using Atlas.Entities;
using Atlas.EventSystem;

using Respawning;

using UnityEngine;

namespace Atlas.Events
{
    public class RespawningTeamEventArgs : BoolEvent
    {
        private SpawnableTeamType nextKnownTeam;

        public RespawningTeamEventArgs(List<Player> players, int maxRespawn, SpawnableTeamType nextKnownTeam, bool isAllowed = true)
        {
            Players = players;
            MaximumRespawnAmount = maxRespawn;
            NextKnownTeam = nextKnownTeam;
            IsAllowed = isAllowed;
        }

        /// <summary>
        /// Gets the list of players that are going to be respawned.
        /// </summary>
        public List<Player> Players { get; }

        /// <summary>
        /// Gets or sets the maximum amount of respawnable players.
        /// </summary>
        public int MaximumRespawnAmount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating what the next respawnable team is.
        /// </summary>
        public SpawnableTeamType NextKnownTeam
        {
            get => nextKnownTeam;
            set
            {
                nextKnownTeam = value;
                ReissueNextKnownTeam();
            }
        }

        internal SpawnableTeamHandlerBase SpawnableTeam => RespawnWaveGenerator.SpawnableTeams.TryGetValue(NextKnownTeam, out SpawnableTeamHandlerBase @base) ? @base : null;

        private void ReissueNextKnownTeam()
        {
            SpawnableTeamHandlerBase @base = SpawnableTeam;

            if (@base == null)
                return;

            int a = RespawnTickets.Singleton.GetAvailableTickets(NextKnownTeam);

            if (a == 0)
            {
                a = RespawnTickets.DefaultTeamAmount;

                RespawnTickets.Singleton.GrantTickets(RespawnTickets.DefaultTeam, RespawnTickets.DefaultTeamAmount, true);
            }

            MaximumRespawnAmount = Mathf.Min(a, @base.MaxWaveSize);
        }
    }
}