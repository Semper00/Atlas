using Atlas.Entities;
using Atlas.Entities.Items;
using Atlas.EventSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Fires before the player reloads his weapon.
    /// </summary>
    public class ReloadingWeapon : BoolEvent
    {
        /// <summary>
        /// Gets the <see cref="FirearmItem"/> being reloaded.
        /// </summary>
        public FirearmItem Weapon { get; }

        /// <summary>
        /// Gets the player who's reloading the weapon.
        /// </summary>
        public Player Player { get; }

        public ReloadingWeapon(Player player, bool allow)
        {
            Weapon = player.CurrentItem as FirearmItem;
            Player = player;
            IsAllowed = allow;
        }
    }
}
