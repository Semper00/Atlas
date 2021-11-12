using Atlas.Entities.Items.Base;

namespace Atlas.Entities.Items.Usable
{
    /// <summary>
    /// Represents the in-game adrenaline.
    /// </summary>
    public class Adrenaline : BaseConsumableItem
    {
        internal InventorySystem.Items.Usables.Adrenaline adrenaline;

        public Adrenaline(InventorySystem.Items.Usables.Adrenaline adrenaline, bool addToApi = false) : base(adrenaline, addToApi)
            => this.adrenaline = adrenaline;
    }
}
