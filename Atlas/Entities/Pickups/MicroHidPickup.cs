using InventorySystem.Items.MicroHID;

using Atlas.Entities.Pickups.Base;

namespace Atlas.Entities.Pickups
{
    /// <summary>
    /// Represents the in-game MicroHID.
    /// </summary>
    public class MicroHidPickup : BasePickup
    {
        internal MicroHIDPickup hid;

        public MicroHidPickup(MicroHIDPickup hid, bool addToApi = false) : base(hid, addToApi)
            => this.hid = hid;

        /// <summary>
        /// Gets or sets the MicroHID's energy.
        /// </summary>
        public float Energy { get => hid.NetworkEnergy; set => hid.NetworkEnergy = value; }
    }
}
