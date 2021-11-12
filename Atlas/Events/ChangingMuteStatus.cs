using Atlas.Entities;
using Atlas.Enums;
using Atlas.EventSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Fires before a player gets muted or unmuted.
    /// </summary>
    public class ChangingMuteStatus : BoolEvent
    {
        /// <summary>
        /// Gets the mute type.
        /// </summary>
        public MuteType Type { get; set; }

        /// <summary>
        /// Gets the current mute status.
        /// </summary>
        public bool Status { get; }

        /// <summary>
        /// Gets or sets the new mute status.
        /// </summary>
        public bool Target { get; set; }

        /// <summary>
        /// Gets the player.
        /// </summary>
        public Player Player { get; }

        public ChangingMuteStatus(Player player, MuteType type, bool status, bool allow)
        {
            Player = player;
            Type = type;
            Target = status;
            Status = player.GetMute(type);
        }
    }
}
