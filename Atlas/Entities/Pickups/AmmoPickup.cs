using Atlas.Enums;
using Atlas.Extensions;
using Atlas.Entities.Pickups.Base;

namespace Atlas.Entities.Pickups
{
    /// <summary>
    /// Represents the in-game ammo pickup.
    /// </summary>
    public class AmmoPickup : BasePickup
    {
        internal InventorySystem.Items.Firearms.Ammo.AmmoPickup ammo;

        public AmmoPickup(InventorySystem.Items.Firearms.Ammo.AmmoPickup ammo, bool addToApi = false) : base(ammo, addToApi)
            => this.ammo = ammo;

        /// <summary>
        /// Gets or sets the amount of ammo.
        /// </summary>
        public ushort Ammo { get => ammo.NetworkSavedAmmo; set => ammo.NetworkSavedAmmo = value; }

        /// <summary>
        /// Gets or sets the ammo's type.
        /// </summary>
        public AmmoType AmmoType { get => Id.GetAmmoType(); set => Id = value.GetItemType(); }
    }
}
