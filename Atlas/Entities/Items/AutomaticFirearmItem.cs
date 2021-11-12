using InventorySystem.Items.Firearms;

namespace Atlas.Entities.Items
{
    /// <summary>
    /// Represents an automatic firearm.
    /// </summary>
    public class AutomaticFirearmItem : FirearmItem
    {
        internal new AutomaticFirearm firearm;

        public AutomaticFirearmItem(AutomaticFirearm firearm, bool addToApi = false) : base(firearm, addToApi)
            => this.firearm = firearm;

        /// <summary>
        /// Gets or sets the firearm's damage type.
        /// </summary>
        public new DamageTypes.DamageType DamageType { get => firearm.DamageType; set => firearm._dmgType = value; }
    }
}
