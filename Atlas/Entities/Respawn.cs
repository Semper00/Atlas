﻿using System.Linq;

using Respawning;

using UnityEngine;

using RespawnEffect = Atlas.Enums.RespawnEffect;

namespace Atlas.Entities
{
    /// <summary>
    /// A set of tools to handle team respawns more easily.
    /// </summary>
    public static class Respawn
    {
        /// <summary>
        /// Gets the next known <see cref="SpawnableTeamType"/> that will spawn.
        /// </summary>
        public static SpawnableTeamType NextKnownTeam => RespawnManager.Singleton.NextKnownTeam;

        /// <summary>
        /// Gets the amount of seconds before the next respawn will occur.
        /// </summary>
        public static int TimeUntilRespawn => Mathf.RoundToInt(RespawnManager.Singleton._timeForNextSequence - (float)RespawnManager.Singleton._stopwatch.Elapsed.TotalSeconds);

        /// <summary>
        /// Gets a value indicating whether or not a team is currently being spawned or the animations are playing for a team.
        /// </summary>
        public static bool IsSpawning => RespawnManager.Singleton._curSequence == RespawnManager.RespawnSequencePhase.PlayingEntryAnimations || RespawnManager.Singleton._curSequence == RespawnManager.RespawnSequencePhase.SpawningSelectedTeam;

        /// <summary>
        /// Gets the amount of spawn tickets belonging to the NTF.
        /// </summary>
        public static int NtfTickets => RespawnTickets.Singleton.GetAvailableTickets(SpawnableTeamType.NineTailedFox);

        /// <summary>
        /// Gets the amount of spawn tickets belonging to the Chaos Insurgency.
        /// </summary>
        public static int ChaosTickets => RespawnTickets.Singleton.GetAvailableTickets(SpawnableTeamType.ChaosInsurgency);

        /// <summary>
        /// Gets the actual <see cref="RespawnEffectsController"/>.
        /// </summary>
        public static RespawnEffectsController Controller => RespawnEffectsController.AllControllers.FirstOrDefault(controller => controller != null);

        /// <summary>
        /// Play an effect when a certain class spawns.
        /// </summary>
        /// <param name="effect">The effect to be played.</param>
        public static void PlayEffect(byte effect) => PlayEffects(new[] { effect });

        /// <summary>
        /// Play an effect when a certain class spawns.
        /// </summary>
        /// <param name="effect">The effect to be played.</param>
        public static void PlayEffect(RespawnEffect effect) => PlayEffects(new[] { effect });

        /// <summary>
        /// Play effects when a certain class spawns.
        /// </summary>
        /// <param name="effects">The effects to be played.</param>
        public static void PlayEffects(byte[] effects) => Controller.RpcPlayEffects(effects);

        /// <summary>
        /// Play effects when a certain class spawns.
        /// </summary>
        /// <param name="effects">The effects to be played.</param>
        public static void PlayEffects(RespawnEffect[] effects) => PlayEffects(effects.Select(effect => (byte)effect).ToArray());

        /// <summary>
        /// Summons the NTF chopper.
        /// </summary>
        public static void SummonNtfChopper() => PlayEffects(new RespawnEffect[] { RespawnEffect.SummonNtfChopper });

        /// <summary>
        /// Summons the <see cref="RoleType.ChaosInsurgency"/> van.
        /// </summary>
        /// <param name="playMusic">Whether or not to play the Chaos Insurgency spawn music.</param>
        public static void SummonChaosInsurgencyVan(bool playMusic = true)
        {
            PlayEffects(playMusic ? new RespawnEffect[]
            {
                RespawnEffect.PlayChaosInsurgencyMusic,
                RespawnEffect.SummonChaosInsurgencyVan,
            }
            :
            new RespawnEffect[]
            {
                RespawnEffect.SummonChaosInsurgencyVan,
            });
        }

        /// <summary>
        /// Grants tickets to a <see cref="SpawnableTeamType"/>.
        /// </summary>
        /// <param name="team">The <see cref="SpawnableTeamType"/> to grant tickets to.</param>
        /// <param name="amount">The amount of tickets to grant.</param>
        /// <param name="overrideLocks">Whether or not to override ticket locks.</param>
        /// <returns>Whether or not tickets were granted successfully.</returns>
        public static bool GrantTickets(SpawnableTeamType team, int amount, bool overrideLocks = false) => RespawnTickets.Singleton.GrantTickets(team, amount, overrideLocks);

        /// <summary>
        /// Forces a spawn of the given <see cref="SpawnableTeamType"/>.
        /// </summary>
        /// <param name="team">The <see cref="SpawnableTeamType"/> to spawn.</param>
        /// <param name="playEffects">Whether or not effects will be played with the spawn.</param>
        public static void ForceWave(SpawnableTeamType team, bool playEffects = false)
        {
            RespawnManager.Singleton.ForceSpawnTeam(team);

            if (playEffects)
            {
                RespawnEffectsController.ExecuteAllEffects(RespawnEffectsController.EffectType.Selection, team);
            }
        }
    }
}
