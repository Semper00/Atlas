using Atlas.Entities.Items.Base;

namespace Atlas.Entities.Items.Usable
{
    /// <summary>
    /// Represents the in-game painkillers.
    /// </summary>
    public class Painkillers : BaseConsumableItem
    {
        internal InventorySystem.Items.Usables.Painkillers painkillers;

        public Painkillers(InventorySystem.Items.Usables.Painkillers painkillers, bool addToApi = false) : base(painkillers, addToApi)
            => this.painkillers = painkillers;
    }
}
