using Atlas.Entities;
using Atlas.EventSystem;
using System;

namespace Atlas.Events
{
    /// <summary>
    /// Fires when someone tries to ban a player.
    /// </summary>
    public class Banning : BoolEvent
    {
        /// <summary>
        /// Gets the player issuing this ban.
        /// </summary>
        public Player Issuer { get; }

        /// <summary>
        /// Gets the player being banned.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets or sets the expiration date.
        /// </summary>
        public DateTime Expiery { get; set; }

        /// <summary>
        /// Gets the issuance date.
        /// </summary>
        public DateTime Issuance { get; }

        /// <summary>
        /// Gets or sets the ban's duration.
        /// </summary>
        public long Duration { get; set; }

        /// <summary>
        /// Gets or sets the ban reason.
        /// </summary>
        public string Reason { get; set; }

        public Banning(Player issuer, Player player, long expiery, long issuance, long duration, string reason, bool allow)
        {
            Issuer = issuer;
            Player = player;

            Expiery = new DateTime(expiery);
            Issuance = new DateTime(issuance);

            Duration = duration;
            IsAllowed = allow;

            Reason = reason;
        }
    }
}
