using Atlas.Entities;
using Atlas.EventSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Fires before a player gets cuffed.
    /// </summary>
    public class Handcuffing : BoolEvent
    {
        /// <summary>
        /// Gets the player that is cuffing the target player.
        /// </summary>
        public Player Cuffer { get; }

        /// <summary>
        /// Gets the player that is being cuffed.
        /// </summary>
        public Player Target { get; }

        public Handcuffing(Player cuffer, Player target, bool allow)
        {
            Cuffer = cuffer;
            Target = target;
            IsAllowed = allow;
        }
    }
}
