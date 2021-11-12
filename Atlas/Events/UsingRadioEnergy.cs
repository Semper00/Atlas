using Atlas.Entities;
using Atlas.Entities.Items;
using Atlas.EventSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Fires before radio battery charge is changed.
    /// </summary>
    public class UsingRadioBattery : BoolEvent
    {
        /// <summary>
        /// Gets the Radio which is being used.
        /// </summary>
        public RadioItem Radio { get; }

        /// <summary>
        /// Gets the player who's using the radio.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets or sets the radio battery charge.
        /// </summary>
        public float Charge { get; set; }

        public UsingRadioBattery(RadioItem radio, Player player, float charge, bool isAllowed = true)
        {
            Radio = radio;
            Player = player;
            Charge = charge;
            IsAllowed = isAllowed;
        }
    }
}
