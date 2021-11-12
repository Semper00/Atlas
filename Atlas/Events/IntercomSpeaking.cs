using Atlas.Entities;
using Atlas.EventSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Fires before a player starts using the Intercom.
    /// </summary>
    public class IntercomSpeaking : BoolEvent
    {
        /// <summary>
        /// Gets the player that is using the Intercom.
        /// </summary>
        public Player Speaker { get; }

        /// <summary>
        /// Gets the <see cref="Entities.Intercom"/> instance.
        /// </summary>
        public Entities.Intercom Intercom { get; }

        public IntercomSpeaking(Player player, Entities.Intercom icom, bool allow)
        {
            Speaker = player;
            Intercom = icom;
            IsAllowed = allow;
        }
    }
}