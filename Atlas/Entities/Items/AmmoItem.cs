using Atlas.Entities.Items.Base;

namespace Atlas.Entities.Items
{
    /// <summary>
    /// Represents the in-game ammo item.
    /// </summary>
    public class AmmoItem : BaseItem
    {
        internal InventorySystem.Items.Firearms.Ammo.AmmoItem ammo;

        public AmmoItem(InventorySystem.Items.Firearms.Ammo.AmmoItem ammo, bool addToApi = false) : base(ammo, addToApi)
            => this.ammo = ammo;

        /// <summary>
        /// Gets or sets the ammo's unit price.
        /// </summary>
        public int UnitPrice { get => ammo.UnitPrice; set => ammo.UnitPrice = value; }
    }
}
