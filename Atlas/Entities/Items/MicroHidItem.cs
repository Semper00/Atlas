using InventorySystem.Items.MicroHID;

using Atlas.Entities.Items.Base;

namespace Atlas.Entities.Items
{
    /// <summary>
    /// Represents the in-game MicroHID.
    /// </summary>
    public class MicroHidItem : BaseItem
    {
        internal MicroHIDItem hid;

        public MicroHidItem(MicroHIDItem hid, bool addToApi = false) : base(hid, addToApi)
            => this.hid = hid;

        /// <summary>
        /// Gets a value indicating whether this MicroHID allows sprinting or not.
        /// </summary>
        public bool AllowSprinting { get => hid.AllowSprinting; }

        /// <summary>
        /// Gets a value indicating whether this MicroHID can be equipped or not.
        /// </summary>
        public bool AllowEquip { get => hid.AllowEquip; }
        
        /// <summary>
        /// Gets a value indicating whether this MicroHID can be holstered or not.
        /// </summary>
        public bool AllowHolster { get => hid.AllowHolster; }

        /// <summary>
        /// Gets the MicroHID's armor penetration value.
        /// </summary>
        public float Penetration { get => hid.ArmorPenetration; }

        /// <summary>
        /// Gets the attacker override role.
        /// </summary>
        public RoleType AttackerOverride { get => hid.AttackerRoleOverride; }

        /// <summary>
        /// Gets or sets the MicroHID's <see cref="HidState"/>.
        /// </summary>
        public HidState State { get => hid.State; set => hid.State = value; }
        
        /// <summary>
        /// Gets or sets the MicroHID's <see cref="HidUserInput"/>.
        /// </summary>
        public HidUserInput UserInput { get => hid.UserInput; set => hid.UserInput = value; }

        /// <summary>
        /// Gets the MicroHID's damage type.
        /// </summary>
        public DamageTypes.DamageType DamageType { get => hid.DamageType; }

        /// <summary>
        /// Gets the movement speed limiter.
        /// </summary>
        public float MovementSpeedLimiter { get => hid.MovementSpeedLimiter; }

        /// <summary>
        /// Gets the movement speed multiplier.
        /// </summary>
        public float MovementSpeedMultiplier { get => hid.MovementSpeedMultiplier; }

        /// <summary>
        /// Gets the stamina usage multiplier.
        /// </summary>
        public float StaminaUsageMultiplier { get => hid.StaminaUsageMultiplier; }

        /// <summary>
        /// Gets or sets the remaining energy.
        /// </summary>
        public float Energy { get => hid.RemainingEnergy; set => hid.RemainingEnergy = value; }

        /// <summary>
        /// Gets the MicroHID's weight.
        /// </summary>
        public float Weight { get => hid.Weight; }

        /// <summary>
        /// Fires the MicroHID.
        /// </summary>
        public void Fire()
            => hid.Fire();

        /// <summary>
        /// Executes the firing sequence.
        /// </summary>
        public void Execute()
            => hid.ExecuteServerside();
    }
}