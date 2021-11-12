using Atlas.Entities;
using Atlas.EventSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Fires when a player gets a new group.
    /// </summary>
    public class ChangingGroup : BoolEvent
    {
        /// <summary>
        /// Gets the player's current group.
        /// </summary>
        public UserGroup Current { get; }

        /// <summary>
        /// Gets or sets the player's new group.
        /// </summary>
        public UserGroup Target { get; set; }

        /// <summary>
        /// Gets the player.
        /// </summary>
        public Player Player { get; }

        public ChangingGroup(Player player, UserGroup target, bool allow)
        {
            Player = player;
            Target = target;
            Current = player.Rank.Group;
        }
    }
}
