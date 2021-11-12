using InventorySystem.Items.Usables;

namespace Atlas.Entities.Items.Base
{
    /// <summary>
    /// A base for usable items.
    /// </summary>
    public class BaseUsableItem : BaseItem
    {
        internal UsableItem item;

        public BaseUsableItem(UsableItem item, bool addToApi = false) : base(item, addToApi)
            => this.item = item;

        /// <summary>
        /// Gets or sets the item's cooldown.
        /// </summary>
        public float Cooldown { get => item.RemainingCooldown; set => item.RemainingCooldown = value; }

        /// <summary>
        /// Gets or sets a value indicating whether this item is being used or not.
        /// </summary>
        public bool IsBeingUsed { get => item.IsUsing; set => item.IsUsing = value; }

        /// <summary>
        /// Removes this item from owner's inventory.
        /// </summary>
        public void Remove()
            => item.ServerRemoveSelf();

        /// <summary>
        /// Sets the item's cooldown.
        /// </summary>
        /// <param name="time">The cooldown.</param>
        /// <param name="isGlobal">Determines if you want to set the cooldown globally or not.</param>
        public void SetCooldown(float time, bool isGlobal = false)
        {
            if (isGlobal)
                item.ServerSetGlobalItemCooldown(time);
            else
                item.ServerSetPersonalCooldown(time);
        }
    }
}
