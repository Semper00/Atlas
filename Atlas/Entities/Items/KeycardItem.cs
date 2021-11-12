using Interactables.Interobjects.DoorUtils;

using Atlas.Entities.Items.Base;

namespace Atlas.Entities.Items
{
    /// <summary>
    /// Represents the in-game keycard.
    /// </summary>
    public class KeycardItem : BaseItem
    {
        internal InventorySystem.Items.Keycards.KeycardItem card;

        public KeycardItem(InventorySystem.Items.Keycards.KeycardItem card, bool addToApi = false) : base(card, addToApi)
            => this.card = card;

        /// <summary>
        /// Gets or sets the keycard's permissions.
        /// </summary>
        public KeycardPermissions Permissions { get => card.Permissions; set => card.Permissions = value; }

        /// <summary>
        /// Gets a value indicating whether this keycard has the specified permission.
        /// </summary>
        /// <param name="perm">The permission to check.</param>
        /// <returns>true if it has the permission, otherwise false.</returns>
        public bool HasPermission(KeycardPermissions perm)
            => Permissions.HasFlagFast(perm);

        /// <summary>
        /// Adds a permission.
        /// </summary>
        /// <param name="perm">The permission to add.</param>
        public void AddPermission(KeycardPermissions perm)
            => Permissions |= perm;


        /// <summary>
        /// Removes a permission.
        /// </summary>
        /// <param name="perm">The permission to remove.</param>
        public void RemovePermission(KeycardPermissions perm)
            => Permissions &= perm;
    }
}
