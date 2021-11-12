using InventorySystem.Items.ThrowableProjectiles;

namespace Atlas.Entities.Items.Grenades
{
    /// <summary>
    /// Represents the SCP-018 grenade.
    /// </summary>
    public class Scp018Grenade : ExplosiveGrenade
    {
        internal new Scp018Projectile nade;

        public Scp018Grenade(Scp018Projectile nade, bool addToApi) : base(nade, addToApi)
            => this.nade = nade;

        /// <summary>
        /// Gets a value indicating whether to ignore friendly fire or not.
        /// </summary>
        public bool IgnoreFriendlyFire { get => nade.IgnoreFriendlyFire; }

        /// <summary>
        /// Gets the current damage.
        /// </summary>
        public float Damage { get => nade.CurrentDamage; }

        /// <summary>
        /// Gets or sets the grenade's cooldown.
        /// </summary>
        public float Cooldown { get => nade._cooldownTimer; set => nade._cooldownTimer = value; }

        /// <summary>
        /// Gets the grenade's activation time.
        /// </summary>
        public float ActivatedTime { get => nade._activatedTime; }

        /// <summary>
        /// Makes a sound.
        /// </summary>
        /// <param name="speed">Grenade speed.</param>
        public void MakeSound(float speed = 10f)
            => nade.RpcMakeSound(speed);
    }
}
