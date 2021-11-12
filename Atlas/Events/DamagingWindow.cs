using Atlas.Entities;
using Atlas.EventSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Fires before a window gets damaged.
    /// </summary>
    public class DamagingWindow : BoolEvent
    {
        /// <summary>
        /// Gets the player that is damaging the window. <b>Can be null.</b>
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the window that is being damaged.
        /// </summary>
        public Window Window { get; }

        /// <summary>
        /// Gets or sets the dealt damage.
        /// </summary>
        public float Damage { get; set; }

        public DamagingWindow(Window window, float damage, bool allow, Player player = null)
        {
            Player = player;
            Window = window;
            Damage = damage;
            IsAllowed = allow;
        }
    }
}
