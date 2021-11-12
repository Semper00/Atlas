using Atlas.Entities;
using Atlas.EventSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Fires when CASSIE announces a SCP termination.
    /// </summary>
    public class AnnouncingScpTermination : BoolEvent
    {
        /// <summary>
        /// Gets the player that killed this SCP. <b>Likely to be null.</b>
        /// </summary>
        public Player Killer { get; }

        /// <summary>
        /// Gets the player that died. <b>Likely to be null.</b>
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the role this SCP died as.
        /// </summary>
        public Role Role { get; }

        /// <summary>
        /// Gets or sets the hit information.
        /// </summary>
        public PlayerStats.HitInfo HitInfo { get; set; }

        public AnnouncingScpTermination(Player killer, Role role, PlayerStats.HitInfo hitInfo, bool allow)
        {
            Killer = killer;
            Role = role;
            HitInfo = hitInfo;
            IsAllowed = allow;
            Player = PlayersList.Get(hitInfo.RHub);
        }
    }
}