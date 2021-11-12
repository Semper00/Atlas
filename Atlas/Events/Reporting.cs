using Atlas.Entities;
using Atlas.Enums;
using Atlas.EventSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Fires before a report gets sent.
    /// </summary>
    public class Reporting : BoolEvent
    {
        /// <summary>
        /// Gets the issuing player.
        /// </summary>
        public Player Issuer { get; }

        /// <summary>
        /// Gets the player who's being reported.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets or sets the report reason.
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// Gets or sets the report's type.
        /// </summary>
        public ReportType Type { get; set; }

        public Reporting(Player issuer, Player player, string reason, ReportType type, bool allow)
        {
            Issuer = issuer;
            Player = player;
            Reason = reason;
            Type = type;
            IsAllowed = allow;
        }
    }
}
