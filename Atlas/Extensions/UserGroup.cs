using System.Linq;

using Atlas.Entities;

namespace Atlas.Extensions
{
    /// <summary>
    /// Contains a useful extension to compare two <see cref="UserGroup"/>'s.
    /// </summary>
    public static class UserGroupExtensions
    {
        /// <summary>
        /// Compares two <see cref="UserGroup"/>'s for equality.
        /// </summary>
        /// <param name="this">The fist <see cref="UserGroup"/>.</param>
        /// <param name="other">The second <see cref="UserGroup"/>.</param>
        /// <returns><c>true</c> if they are equal; otherwise, <c>false</c>.</returns>
        public static bool EqualsTo(this UserGroup @this, UserGroup other)
            => @this.BadgeColor == other.BadgeColor
               && @this.BadgeText == other.BadgeText
               && @this.Permissions == other.Permissions
               && @this.Cover == other.Cover
               && @this.HiddenByDefault == other.HiddenByDefault
               && @this.Shared == other.Shared
               && @this.KickPower == other.KickPower
               && @this.RequiredKickPower == other.RequiredKickPower;

        /// <summary>
        /// Searches for a key of a group in the <see cref="PermissionsHandler">RemoveAdmin</see> config.
        /// </summary>
        /// <param name="this">The <see cref="UserGroup"/>.</param>
        /// <returns>The key of that group, or null if not found.</returns>
        public static string GetKey(this UserGroup @this) => Server.PermissionsHandler._groups
            .FirstOrDefault(pair => pair.Value.EqualsTo(@this)).Key;
    }
}
