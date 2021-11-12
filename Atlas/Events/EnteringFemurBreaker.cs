using Atlas.Entities;
using Atlas.EventSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Fires before a player enters the femur breaker.
    /// </summary>
    public class EnteringFemurBreaker : BoolEvent
    {
        /// <summary>
        /// Gets the player that is entering the femur breaker.
        /// </summary>
        public Player Player { get; }

        public EnteringFemurBreaker(Player player, bool allow)
        {
            Player = player;
            IsAllowed = allow;
        }
    }
}
