using Atlas.Entities;
using Atlas.Enums;
using Atlas.Entities.Items;
using Atlas.EventSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Fires before MicroHID energy is changed.
    /// </summary>
    public class UsingMicroHidEnergy : BoolEvent
    {
        /// <summary>
        /// Gets the player who's using the MicroHID.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the MicroHID instance.
        /// </summary>
        public MicroHidItem Micro { get; }

        /// <summary>
        /// Gets the current state of the MicroHID.
        /// </summary>
        public HidState CurrentState { get; }

        /// <summary>
        /// Gets the old MicroHID energy value.
        /// </summary>
        public float OldValue { get; }

        /// <summary>
        /// Gets or sets the new MicroHID energy value.
        /// </summary>
        public float NewValue { get; set; }

        public UsingMicroHidEnergy(Player player, MicroHidItem micro, HidState currentState, float oldValue, float newValue, bool allow)
        {
            Player = player;
            Micro = micro;
            CurrentState = currentState;
            OldValue = oldValue;
            NewValue = newValue;
            IsAllowed = allow;
        }
    }
}
