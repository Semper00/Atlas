namespace Atlas.Entities
{
    /// <summary>
    /// A class for easier rank/badge management.
    /// </summary>
    public class Rank
    {
        private Player _player;

        /// <summary>
        /// Initialzes a new instance of the <see cref="Rank"/> class.
        /// </summary>
        /// <param name="player">The owner of this rank.</param>
        public Rank(Player player) => _player = player;

        /// <summary>
        /// Gets or sets the rank name.
        /// </summary>
        public string Text { get => _player?.Hub.serverRoles.Group?.BadgeText ?? ""; set => _player?.Hub.serverRoles.SetText(value); }

        /// <summary>
        /// Gets the global rank name.
        /// </summary>
        public string GlobalText { get => _player?.Hub.serverRoles._bgt; }

        /// <summary>
        /// Gets or sets the rank color.
        /// </summary>
        public string Color { get => _player?.Hub.serverRoles.Group?.BadgeColor ?? "default"; set => _player?.Hub.serverRoles.SetColor(value); }

        /// <summary>
        /// Gets the global rank color.
        /// </summary>
        public string GlobalColor { get => _player?.Hub.serverRoles._bgc; }

        /// <summary>
        /// Gets the rank color in a hexadecimal code.
        /// </summary>
        public string HexValue { get => _player?.Hub.serverRoles.CurrentColor?.ColorHex ?? ""; set => Color = value; }

        /// <summary>
        /// Gets the remote admin config key for this rank.
        /// </summary>
        public string Key { get => ServerStatic.PermissionsHandler._members.TryGetValue(_player?.UserId, out string groupName) ? groupName : ""; set =>
                ServerStatic.PermissionsHandler._members[_player.UserId] = value; }

        /// <summary>
        /// Gets the rank type.
        /// </summary>
        public int Type { get => _player?.Hub.serverRoles.GlobalBadgeType ?? 0; }

        /// <summary>
        /// Gets a value indicating whether this role is global or not.
        /// </summary>
        public bool IsGlobal { get => !string.IsNullOrWhiteSpace(_player?.Hub.serverRoles.GlobalBadge); }

        /// <summary>
        /// Gets or sets a value indicating whether this rank is hidden or not.
        /// </summary>
        public bool IsHidden
        {
            get => string.IsNullOrEmpty(_player?.Hub.serverRoles.HiddenBadge);
            set
            {
                if (value)
                {
                    if (_player != null)
                        _player.Hub.serverRoles.HiddenBadge = Text;
                }
                else
                    _player.Hub.serverRoles.HiddenBadge = string.Empty;
            }
        }

        /// <summary>
        /// Gets the owner of this rank.
        /// </summary>
        public Player Owner { get => _player; }

        /// <summary>
        /// Gets or sets the player's group.
        /// </summary>
        public UserGroup Group { get => _player.Hub.serverRoles.Group; set => _player.Hub.serverRoles.SetGroup(value, false); }
    }
}