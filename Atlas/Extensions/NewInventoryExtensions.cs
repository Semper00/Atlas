using Atlas.Entities.Items.Base;

using InventorySystem;

namespace Atlas.Extensions
{
    public static class NewInventoryExtensions
    {
        /// <summary>
        /// Sets the current item.
        /// </summary>
        /// <param name="inv"></param>
        /// <param name="value"></param>
        public static void SetItem(this Inventory inv, BaseItem value)
        {
            if (!inv.UserInventory.Items.TryGetValue(value.Serial, out _))
                inv.ServerAddItem(value.Id, value.Serial, value.Base.PickupDropModel);

            inv.ServerSelectItem(value.Serial);
        }
    }
}
