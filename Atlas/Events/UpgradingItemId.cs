using Atlas.EventSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Fires when SCP-914 tries to upgrade an item's ID (mostly when upgrading inventory items).
    /// </summary>
    public class UpgradingItemId : BoolEvent
    {
        /// <summary>
        /// Gets the item's current ID.
        /// </summary>
        public ItemType Id { get; }

        /// <summary>
        /// Gets or sets the item's target ID.
        /// </summary>
        public ItemType TargetId { get; set; }

        public UpgradingItemId(ItemType id, ItemType target, bool allow)
        {
            Id = id;
            TargetId = target;
            IsAllowed = allow;
        }
    }
}
