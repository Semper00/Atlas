using InventorySystem.Items.Firearms;

using Atlas.Entities.Pickups.Base;

namespace Atlas.Entities.Pickups
{
    /// <summary>
    /// Represents an in-game gun pickup.
    /// </summary>
    public class FirearmPickup : BasePickup
    {
        internal InventorySystem.Items.Firearms.FirearmPickup firearm;

        public FirearmPickup(InventorySystem.Items.Firearms.FirearmPickup firearm, bool addToApi = false) : base(firearm, addToApi)
            => this.firearm = firearm;

        /// <summary>
        /// Gets or sets a value indicating whether this pickup has been distributed or not.
        /// </summary>
        public bool IsDistributed { get => firearm.Distributed; set => firearm.Distributed = value; }

        /// <summary>
        /// Gets or sets the firearm's attachments.
        /// </summary>
        public FirearmWorldmodel.AttachmentElement[] Attachments { get => firearm._worldmodel._attachments; set => firearm._worldmodel._attachments = value; }

        /// <summary>
        /// Gets or sets the firearm's status.
        /// </summary>
        public FirearmStatus Status { get => firearm.NetworkStatus; set => firearm.NetworkStatus = value; }

        /// <summary>
        /// Gets or sets the firearm's ammo.
        /// </summary>
        public byte Ammo { get => Status.Ammo; set => Status = new FirearmStatus(value, Status.Flags, Status.Attachments); }
    }
}
