using Atlas.Entities.Items.Base;

namespace Atlas.Entities.Items.Usable
{
    /// <summary>
    /// Represents the in-game SCP-207.
    /// </summary>
    public class Scp207 : BaseConsumableItem
    {
        internal InventorySystem.Items.Usables.Scp207 scp;

        public Scp207(InventorySystem.Items.Usables.Scp207 scp, bool addToApi = false) : base(scp, addToApi)
            => this.scp = scp;
    }
}
