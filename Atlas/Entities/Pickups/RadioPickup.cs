using Atlas.Enums;
using Atlas.Entities.Pickups.Base;

namespace Atlas.Entities.Pickups
{
    /// <summary>
    /// Represents the in-game radio pickup.
    /// </summary>
    public class RadioPickup : BasePickup
    {
        internal InventorySystem.Items.Radio.RadioPickup radio;

        public RadioPickup(InventorySystem.Items.Radio.RadioPickup radio, bool addToApi = false) : base(radio, addToApi)
            => this.radio = radio;

        /// <summary>
        /// Gets or sets a value indicating whether the radio is enabled or not.
        /// </summary>
        public bool IsEnabled { get => radio.SavedEnabled; set => radio.SavedEnabled = value; }

        /// <summary>
        /// Gets or sets the battery's percentage.
        /// </summary>
        public float Battery { get => radio.SavedBattery; set => radio.SavedBattery = value; }

        /// <summary>
        /// Gets or sets the radio's range.
        /// </summary>
        public RadioLevel Range { get => (RadioLevel)radio.SavedRange; set => radio.SavedRange = (int)value; }
    }
}