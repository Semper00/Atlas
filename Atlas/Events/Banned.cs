using Atlas.Entities;
using Atlas.EventSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Fires AFTER a player gets banned.
    /// </summary>
    public class Banned : Event
    {
        /// <summary>
        /// Gets the player issuing this ban. <b>May be null.</b>
        /// </summary>
        public Player Issuer { get; }

        /// <summary>
        /// Gets or sets the ban's details.
        /// </summary>
        public BanDetails Details { get; set; }

        public Banned(Player issuer, BanDetails details)
        {
            Issuer = issuer;
            Details = details;
        }
    }
}
