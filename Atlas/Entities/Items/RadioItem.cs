using InventorySystem.Items.Radio;

using Atlas.Entities.Items.Base;

using Atlas.Enums;

namespace Atlas.Entities.Items
{
    /// <summary>
    /// Represents an in-game inventory radio.
    /// </summary>
    public class RadioItem : BaseItem
    {
        internal InventorySystem.Items.Radio.RadioItem radio;

        public RadioItem(InventorySystem.Items.Radio.RadioItem radio, bool addToApi = false) : base(radio, addToApi)
            => this.radio = radio;

        /// <summary>
        /// Gets a value indicating whether this radio is usable or not.
        /// </summary>
        public bool IsUsable { get => radio.IsUsable; }

        /// <summary>
        /// Gets or sets the battery's percentage.
        /// </summary>
        public byte Battery { get => radio.BatteryPercent; set => radio.BatteryPercent = value; }

        /// <summary>
        /// Gets or sets the current radio range.
        /// </summary>
        public RadioLevel Mode { get => (RadioLevel)radio.CurRange; set => radio.CurRange = (int)value; }

        /// <summary>
        /// Gets or sets the current range ID.
        /// </summary>
        public byte RangeId { get => radio._radio.NetworkcurRangeId; set => radio._radio.NetworkcurRangeId = value; }

        /// <summary>
        /// Gets the <see cref="global::Radio"/> component.
        /// </summary>
        public Radio Radio { get => radio._radio; }

        /// <summary>
        /// Gets an array of all radio ranges.
        /// </summary>
        public RadioRangeMode[] Modes { get => radio.Ranges; set => radio.Ranges = value; }

        /// <summary>
        /// Disables this radio.
        /// </summary>
        public void Disable()
            => radio.ServerProcessCmd(RadioMessages.RadioCommand.Disable);

        /// <summary>
        /// Enables this radio.
        /// </summary>
        public void Enable()
            => radio.ServerProcessCmd(RadioMessages.RadioCommand.Enable);

        /// <summary>
        /// Switches to the next mode.
        /// </summary>
        public void ChangeMode()
            => radio.ServerProcessCmd(RadioMessages.RadioCommand.ChangeRange);
    }
}