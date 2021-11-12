using Atlas.Entities;
using Atlas.EventSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Fires before a player interacts with a locker.
    /// </summary>
    public class InteractingLocker : BoolEvent
    {
        /// <summary>
        /// Gets the player who's interacting with the locker.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the <see cref="Locker"/> instance.
        /// </summary>
        public Locker Locker { get; }

        /// <summary>
        /// Gets the interacting chamber.
        /// </summary>
        public Chamber Chamber { get; }

        /// <summary>
        /// Gets the locker id.
        /// </summary>
        public byte LockerId { get; }

        /// <summary>
        /// Gets the chamber id.
        /// </summary>
        public byte ChamberId { get; }

        public InteractingLocker(Player player, Locker locker, Chamber chamber, byte lockerId, byte chamberId, bool allow)
        {
            Player = player;
            Locker = locker;
            Chamber = chamber;
            LockerId = lockerId;
            ChamberId = chamberId;
            IsAllowed = allow;
        }
    }
}
