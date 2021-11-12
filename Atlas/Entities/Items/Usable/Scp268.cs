using Atlas.Entities.Items.Base;

namespace Atlas.Entities.Items.Usable
{
    /// <summary>
    /// Represents the in-game SCP-268.
    /// </summary>
    public class Scp268 : BaseUsableItem
    {
        internal InventorySystem.Items.Usables.Scp268 scp;

        public Scp268(InventorySystem.Items.Usables.Scp268 scp, bool addToApi = false) : base(scp, addToApi)
            => this.scp = scp;

        /// <summary>
        /// Gets or sets a value indicating whether someone is using this item or not.
        /// </summary>
        public bool IsWorn { get => scp.IsWorn; set => scp.IsWorn = value; }

        /// <summary>
        /// Enables the SCP-268 effect.
        /// </summary>
        public void Enable()
            => scp.SetState(true);

        /// <summary>
        /// Disables the SCP-268 effect.
        /// </summary>
        public void Disable()
            => scp.SetState(false);
    }
}
