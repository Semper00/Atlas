using UnityEngine;

using Atlas.Enums;

namespace Atlas.Extensions
{
    /// <summary>
    /// A set of extensions for <see cref="RoleType"/>.
    /// </summary>
    public static class RoleExtensions
    {
        /// <summary>
        /// Get a <see cref="RoleType">role's</see> <see cref="Color"/>.
        /// </summary>
        /// <param name="role">The <see cref="RoleType"/> to get the color of.</param>
        /// <returns>The <see cref="Color"/> of the role.</returns>
        public static Color GetColor(this RoleType role) => role == RoleType.None ? Color.white : CharacterClassManager._staticClasses.Get(role).classColor;

        /// <summary>
        /// Get the <see cref="Team"/> of the given <see cref="RoleType"/>.
        /// </summary>
        /// <param name="roleType">Role.</param>
        /// <returns><see cref="Team"/>.</returns>
        public static Team GetTeam(this RoleType roleType)
        {
            switch (roleType)
            {
                case RoleType.ChaosConscript:
                case RoleType.ChaosMarauder:
                case RoleType.ChaosRepressor:
                case RoleType.ChaosRifleman:
                    return Team.CHI;
                case RoleType.Scientist:
                    return Team.RSC;
                case RoleType.ClassD:
                    return Team.CDP;
                case RoleType.Scp049:
                case RoleType.Scp93953:
                case RoleType.Scp93989:
                case RoleType.Scp0492:
                case RoleType.Scp079:
                case RoleType.Scp096:
                case RoleType.Scp106:
                case RoleType.Scp173:
                    return Team.SCP;
                case RoleType.Spectator:
                    return Team.RIP;
                case RoleType.FacilityGuard:
                case RoleType.NtfCaptain:
                case RoleType.NtfPrivate:
                case RoleType.NtfSergeant:
                case RoleType.NtfSpecialist:
                    return Team.MTF;
                case RoleType.Tutorial:
                    return Team.TUT;
                default:
                    return Team.RIP;
            }
        }

        /// <summary>
        /// Gets the leading team of a team.
        /// </summary>
        /// <param name="team">The team.</param>
        /// <returns>The leading team of the team.</returns>
        public static LeadingTeam GetLeadingTeam(this Team team)
        {
            switch (team)
            {
                case Team.CDP:
                case Team.CHI:
                    return LeadingTeam.ChaosInsurgency;
                case Team.MTF:
                case Team.RSC:
                    return LeadingTeam.FacilityForces;
                case Team.RIP:
                case Team.TUT:
                    return LeadingTeam.Draw;
                case Team.SCP:
                    return LeadingTeam.Anomalies;
                default:
                    return LeadingTeam.Draw;
            }
        }

        /// <summary>
        /// Gets a random spawn point of a <see cref="RoleType"/>.
        /// </summary>
        /// <param name="roleType">The <see cref="RoleType"/> to get the spawn point from.</param>
        /// <returns>Returns the spawn point <see cref="Vector3"/>.</returns>
        public static Vector3 GetRandomSpawnPoint(this RoleType roleType)
        {
            GameObject randomPosition = CharacterClassManager._spawnpointManager.GetRandomPosition(roleType);

            return randomPosition == null ? Vector3.zero : randomPosition.transform.position;
        }
    }
}
