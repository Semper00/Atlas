using Atlas.Entities;
using Atlas.Entities.Items.Base;

namespace Atlas.Events
{
    public class UsingItem : UsedItem
    { 
        /// <summary>
        /// Gets or sets the medical item cooldown.
        /// </summary>
        public float Cooldown { get; set; }

        public UsingItem(Player player, BaseUsableItem item, float cooldown) : base(player, item)
        {
            Cooldown = cooldown;
        }
    }
}
