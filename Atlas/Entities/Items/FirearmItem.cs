using CameraShaking;

using InventorySystem.Items.Firearms;
using InventorySystem.Items.Firearms.Attachments;
using InventorySystem.Items.Firearms.Modules;

using Atlas.Enums;
using Atlas.Extensions;
using Atlas.Entities.Items.Base;

using UnityEngine;

using System;

namespace Atlas.Entities.Items
{
    /// <summary>
    /// Represents an in-game firearm inventory item.
    /// </summary>
    public class FirearmItem : BaseItem
    {
        internal Firearm firearm;

        public FirearmItem(Firearm firearm, bool addToApi = false) : base(firearm, addToApi)
        {
            this.firearm = firearm;

            if (BulletHolePrefab == null)
                BulletHolePrefab = firearm.BulletHolePrefab;
        }

        /// <summary>
        /// Gets the firearm's <see cref="IActionModule"/>.
        /// </summary>
        public IActionModule ActionModule { get => firearm.ActionModule; }

        /// <summary>
        /// Gets the firearm's <see cref="IAdsModule"/>.
        /// </summary>
        public IAdsModule AdsModule { get => firearm.AdsModule; }

        /// <summary>
        /// Gets the firearm's <see cref="IAmmoManagerModule"/>.
        /// </summary>
        public IAmmoManagerModule AmmoManager { get => firearm.AmmoManagerModule; }

        /// <summary>
        /// Gets or sets the firearm's <see cref="IEquipperModule"/>.
        /// </summary>
        public IEquipperModule EquipperModule { get => firearm.EquipperModule; set => firearm.EquipperModule = value; }

        /// <summary>
        /// Gets or sets the firearm's <see cref="IHitregModule"/>.
        /// </summary>
        public IHitregModule HitregModule { get => firearm.HitregModule; set => firearm.HitregModule = value; }

        /// <summary>
        /// Gets or sets the firearm's <see cref="IInspectorModule"/>.
        /// </summary>
        public IInspectorModule InspectorModule { get => firearm.InspectorModule; set => firearm.InspectorModule = value; }

        /// <summary>
        /// Gets the firearm's <see cref="Enums.AmmoType"/>.
        /// </summary>
        public AmmoType AmmoType { get => firearm.AmmoType.GetAmmoType(); }

        /// <summary>
        /// Gets the firearm's attacker role override.
        /// </summary>
        public RoleType AttackerOverride { get => firearm.AttackerRoleOverride; }

        /// <summary>
        /// Gets or sets the firearm's affiliation.
        /// </summary>
        public Faction Affiliation { get => firearm.FirearmAffiliation; set => firearm.FirearmAffiliation = value; }

        /// <summary>
        /// Gets or sets the firearm's status.
        /// </summary>
        public FirearmStatus Status { get => firearm.Status; set => firearm.Status = value; }

        /// <summary>
        /// Gets the firearm's damage statistics.
        /// </summary>
        public FirearmBaseStats Damage { get => firearm.BaseStats; }

        /// <summary>
        /// Gets the firearm's damage type.
        /// </summary>
        public DamageTypes.DamageType DamageType { get => firearm.DamageType; }

        /// <summary>
        /// Gets or sets the firearm's attachments.
        /// </summary>
        public FirearmAttachment[] Attachments { get => firearm.Attachments; set => firearm.Attachments = value; }

        /// <summary>
        /// Gets or sets the firearm's attachment settings.
        /// </summary>
        public AttachmentSettings AttachmentSettings { get => firearm.CombinedAttachments; set => firearm.CombinedAttachments = value; }

        /// <summary>
        /// Gets a value indicating whether you can disarm players with this firearm or not.
        /// </summary>
        public bool AllowDisarming { get => firearm.AllowDisarming; }

        /// <summary>
        /// Gets a value indicating whether you can sprint with this firearm or not.
        /// </summary>
        public bool AllowSprinting { get => firearm.AllowSprinting; }

        /// <summary>
        /// Gets a value indicating whether this firearm is emitting light or not.
        /// </summary>
        public bool IsEmittingLight { get => firearm.IsEmittingLight; }

        /// <summary>
        /// Gets a value indicating whether this firearm is automatic or not..
        /// </summary>
        public bool IsAutomatic { get => Base is AutomaticFirearm auto && auto != null; }

        /// <summary>
        /// Gets a value indicating whether this firearm uses SCP hitbox multipliers or not.
        /// </summary>
        public bool UseScpHitboxMultiplier { get => firearm.UseScpHitboxMultipliers; }

        /// <summary>
        /// Gets a value indicating whether this firearm uses hitbox multipliers or not.
        /// </summary>
        public bool UseHitboxMultiplier { get => firearm.UseHumanHitboxMultipliers; }

        /// <summary>
        /// Gets the firearm's armor penetration value.
        /// </summary>
        public float Penetration { get => firearm.ArmorPenetration; }

        /// <summary>
        /// Gets the firearm's movement speed limiter.
        /// </summary>
        public float MovementSpeedLimiter { get => firearm.MovementSpeedLimiter; }

        /// <summary>
        /// Gets the firearm's movement speed multiplier.
        /// </summary>
        public float MovementSpeedMultiplier { get => firearm.MovementSpeedMultiplier; }

        /// <summary>
        /// Gets the firearm's stamina usage multiplier.
        /// </summary>
        public float StaminaUsageMultiplier { get => firearm.StaminaUsageMultiplier; }

        /// <summary>
        /// Gets or sets the firearm's fire rate.
        /// </summary>
        public float FireRate
        {
            get => Base is AutomaticFirearm auto ? auto._fireRate : 1f;
            set
            {
                if (Base is AutomaticFirearm auto)
                    auto._fireRate = value;
                else
                    throw new InvalidOperationException("You can't set the fire rate of non-automatic weapons.");
            }
        }

        /// <summary>
        /// Gets or sets the firearm's recoil.
        /// </summary>
        public RecoilSettings Recoil
        {
            get => Base is AutomaticFirearm auto ? auto._recoil : default;
            set
            {
                if (Base is AutomaticFirearm auto)
                    auto.ActionModule = new AutomaticAction(firearm, auto._semiAutomatic, auto._boltTravelTime, 1f / auto._fireRate, auto._dryfireClipId, auto._triggerClipId, auto._gunshotPitchRandomization, value, auto._recoilPattern, auto._hasBoltLock);
            }
        }

        /// <summary>
        /// Gets or sets the amount of ammo.
        /// </summary>
        public byte Ammo { get => Status.Ammo; set => Status = new FirearmStatus(value, Status.Flags, Status.Attachments); }

        /// <summary>
        /// Gets the maximum amount of ammo.
        /// </summary>
        public byte MaxAmmo { get => AmmoManager.MaxAmmo; }

        /// <summary>
        /// Reloads this firearm.
        /// </summary>
        public void Reload()
            => AmmoManager.ServerTryReload();

        /// <summary>
        /// Unloads ammo from this firearm.
        /// </summary>
        public void Unload()
            => AmmoManager.ServerTryUnload();

        /// <summary>
        /// Gets the server's bullet hole prefab.
        /// </summary>
        public static GameObject BulletHolePrefab { get; internal set; }
    }
}
