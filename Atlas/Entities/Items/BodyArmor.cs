using Atlas.Entities.Items.Base;

namespace Atlas.Entities.Items
{
    /// <summary>
    /// Represents the in-game body armor.
    /// </summary>
    public class BodyArmor : BaseItem
    {
        internal InventorySystem.Items.Armor.BodyArmor armor;

        public BodyArmor(InventorySystem.Items.Armor.BodyArmor armor, bool addToApi = false) : base(armor, addToApi)
            => this.armor = armor;

        /// <summary>
        /// Gets a value indicating whether this armor can be equiped or not.
        /// </summary>
        public bool AllowEquip { get => armor.AllowEquip; }

        /// <summary>
        /// Gets a value indicating whether this armor can be holstered or not.
        /// </summary>
        public bool AllowHolster { get => armor.AllowHolster; }

        /// <summary>
        /// Gets a value indicating whether someone is wearing this armor or not.
        /// </summary>
        public bool IsWorn { get => armor.IsWorn; }

        /// <summary>
        /// Gets or sets a value indicating whether to drop excessive items and ammo on drop or not.
        /// </summary>
        public bool RemoveExcessOnDrop { get => !armor.DontRemoveExcessOnDrop; set => armor.DontRemoveExcessOnDrop = !value; }

        /// <summary>
        /// Gets or sets the armor's ammo limits.
        /// </summary>
        public InventorySystem.Items.Armor.BodyArmor.ArmorAmmoLimit[] AmmoLimits { get => armor.AmmoLimits; set => armor.AmmoLimits = value; }

        /// <summary>
        /// Gets or sets the armor's item category limits.
        /// </summary>
        public InventorySystem.Items.Armor.BodyArmor.ArmorCategoryLimitModifier[] CategoryLimits { get => armor.CategoryLimits; set => armor.CategoryLimits = value; }

        /// <summary>
        /// Gets or sets the multiplier for downsides of civilian classes.
        /// </summary>
        public float CivilianDownsidesMultiplier { get => armor.CivilianClassDownsidesMultiplier; set => armor.CivilianClassDownsidesMultiplier = value; }

        /// <summary>
        /// Gets or sets the movement speed multiplier.
        /// </summary>
        public float MovementSpeedMultiplier { get => armor.MovementSpeedMultiplier; set => armor.MovementSpeedMultiplier = value; }

        /// <summary>
        /// Gets or sets the stamina use multiplier.
        /// </summary>
        public float StaminaUseMultiplier { get => armor.MovementSpeedMultiplier; set => armor.MovementSpeedMultiplier = value; }

        /// <summary>
        /// Gets or sets the armor's weight.
        /// </summary>
        public float Weight { get => armor.Weight; set => armor._weight = value; }

        /// <summary>
        /// Gets or sets the helmet's efficacy.
        /// </summary>
        public int HelmetEfficacy { get => armor.HelmetEfficacy; set => armor.HelmetEfficacy = value; }

        /// <summary>
        /// Gets or sets the vest's efficacy.
        /// </summary>
        public int VestEfficacy { get => armor.VestEfficacy; set => armor.VestEfficacy = value; }
    }
}
