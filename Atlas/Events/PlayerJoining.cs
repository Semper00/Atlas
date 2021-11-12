using Atlas.Entities;
using Atlas.EventSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Fires when a player fully connects.
    /// </summary>
    public class PlayerJoining : BoolEvent
    {
        /// <summary>
        /// Gets the player that joined.
        /// </summary>
        public Player Player { get; }

        public PlayerJoining(Player player)
            => Player = player;

        /// <summary>
        /// Disconnects the player.
        /// </summary>
        /// <param name="message">Disconnection reason.</param>
        public void Disallow(string message = null)
            => Player.Disconnect(message == null ? "No reason specified." : message);
    }
}
