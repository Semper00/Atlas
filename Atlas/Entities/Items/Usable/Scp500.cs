using Atlas.Entities.Items.Base;

namespace Atlas.Entities.Items.Usable
{
    /// <summary>
    /// Represents the in-game SCP-500.
    /// </summary>
    public class Scp500 : BaseConsumableItem
    {
        internal InventorySystem.Items.Usables.Scp500 scp;

        public Scp500(InventorySystem.Items.Usables.Scp500 scp, bool addToApi = false) : base(scp, addToApi)
            => this.scp = scp;
    }
}