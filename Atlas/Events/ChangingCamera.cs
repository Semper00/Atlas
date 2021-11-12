using Atlas.Entities;
using Atlas.EventSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Fires when SCP-079 tries to switch cameras.
    /// </summary>
    public class ChangingCamera : BoolEvent
    {
        /// <summary>
        /// Gets the current camera.
        /// </summary>
        public Camera Current { get; }

        /// <summary>
        /// Gets or sets the new camera.
        /// </summary>
        public Camera Target { get; set; }

        /// <summary>
        /// Gets the player controlling SCP-079.
        /// </summary>
        public Player Player { get; }

        public ChangingCamera(Player player, Camera cam, bool allow)
        {
            Current = player.Camera;
            Target = cam;
            Player = player;
            IsAllowed = allow;
        }
    }
}
