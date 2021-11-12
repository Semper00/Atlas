using Atlas.Entities;
using Atlas.EventSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Fires AFTER a player changes roles.
    /// </summary>
    public class ChangedRole : Event
    {
        /// <summary>
        /// Gets the player that changed role.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets or sets the new role.
        /// </summary>
        public RoleType OldRole { get; }

        /// <summary>
        /// Gets the old cuffer's ID.
        /// </summary>
        public int OldCufferId { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the player has escaped or not.
        /// </summary>
        public bool IsEscaped { get; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to save position.
        /// </summary>
        public bool SavePosition { get; }

        public ChangedRole(Player player, RoleType role, int cuffer, bool escaped, bool position)
        {
            Player = player;
            OldRole = role;
            OldCufferId = cuffer;
            IsEscaped = escaped;
            SavePosition = position;
        }
    }
}
