using InventorySystem.Items.Flashlight;

using Atlas.Entities.Items.Base;

namespace Atlas.Entities.Items
{
    /// <summary>
    /// Represents the in-game flashlight.
    /// </summary>
    public class Flashlight : BaseItem
    {
        internal FlashlightItem flashlight;

        public Flashlight(FlashlightItem flashlight, bool addToApi = false) : base(flashlight, addToApi)
            => this.flashlight = flashlight;

        /// <summary>
        /// Gets or sets a value indicating whether the flashlight is enabled or not.
        /// </summary>
        public bool IsEnabled { get => flashlight.IsEmittingLight; set => flashlight.IsEmittingLight = value; }

        /// <summary>
        /// Enables the flashlight.
        /// </summary>
        public void Enable()
            => IsEnabled = true;

        /// <summary>
        /// Disables the flashlight
        /// </summary>
        public void Disable()
            => IsEnabled = false;
    }
}
