using InventorySystem.Items.Usables;

namespace Atlas.Entities.Items.Base
{
    /// <summary>
    /// Base for consumable items.
    /// </summary>
    public class BaseConsumableItem : BaseUsableItem
    {
        internal new Consumable item;

        public BaseConsumableItem(Consumable item, bool addToApi = false) : base(item, addToApi)
            => this.item = item;

        /// <summary>
        /// Gets a value indicating whether the item is activation ready or not.
        /// </summary>
        public bool IsReady { get => item.ActivationReady; }

        /// <summary>
        /// Gets or sets the item's usage progress.
        /// </summary>
        public float Progress { get => item.ProgressbarValue; set => item.ProgressbarValue = value; }

        /// <summary>
        /// Completes the item's usage.
        /// </summary>
        public void CompleteUsage()
            => item.ServerOnUsingCompleted();

        /// <summary>
        /// Activates all item's effects.
        /// </summary>
        public void ActivateEffects()
            => item.ActivateEffects();
    }
}
