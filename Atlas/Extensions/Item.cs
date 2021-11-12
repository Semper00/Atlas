﻿using System;
using System.Linq;

using UnityEngine;

using Atlas.Enums;
using Atlas.Entities;

using InventorySystem;

namespace Atlas.Extensions
{
    /// <summary>
    /// A set of extensions for <see cref="ItemType"/>.
    /// </summary>
    public static class ItemExtensions
    {
        /// <summary>
        /// Check if an <see cref="ItemType">item</see> is an ammo.
        /// </summary>
        /// <param name="item">The item to be checked.</param>
        /// <returns>Returns whether the <see cref="ItemType"/> is an ammo or not.</returns>
        public static bool IsAmmo(this ItemType item) => item == ItemType.Ammo9x19 || item == ItemType.Ammo12gauge || item == ItemType.Ammo44cal || item == ItemType.Ammo556x45 || item == ItemType.Ammo762x39;

        /// <summary>
        /// Check if an <see cref="ItemType">item</see> is a weapon.
        /// </summary>
        /// <param name="type">The item to be checked.</param>
        /// <param name="checkMicro">Indicates whether the MicroHID item should be taken into account or not.</param>
        /// <returns>Returns whether the <see cref="ItemType"/> is a weapon or not.</returns>
        public static bool IsWeapon(this ItemType type, bool checkMicro = true) => type == ItemType.GunCrossvec ||
            type == ItemType.GunLogicer || type == ItemType.GunRevolver || type == ItemType.GunShotgun ||
            type == ItemType.GunAK || type == ItemType.GunCOM15 || type == ItemType.GunCOM18 ||
            type == ItemType.GunE11SR || type == ItemType.GunFSP9 || (checkMicro && type == ItemType.MicroHID);

        /// <summary>
        /// Check if an <see cref="ItemType">item</see> is an SCP.
        /// </summary>
        /// <param name="type">The item to be checked.</param>
        /// <returns>Returns whether the <see cref="ItemType"/> is an SCP or not.</returns>
        public static bool IsScp(this ItemType type) => type == ItemType.SCP018 || type == ItemType.SCP500 || type == ItemType.SCP268 || type == ItemType.SCP207;

        /// <summary>
        /// Check if an <see cref="ItemType">item</see> is a throwable item.
        /// </summary>
        /// <param name="type">The item to be checked.</param>
        /// <returns>Returns whether the <see cref="ItemType"/> is a throwable item or not.</returns>
        public static bool IsThrowable(this ItemType type) => type == ItemType.SCP018 || type == ItemType.GrenadeHE || type == ItemType.GrenadeFlash;

        /// <summary>
        /// Check if an <see cref="ItemType">item</see> is a medical item.
        /// </summary>
        /// <param name="type">The item to be checked.</param>
        /// <returns>Returns whether the <see cref="ItemType"/> is a medical item or not.</returns>
        public static bool IsMedical(this ItemType type) => type == ItemType.Painkillers || type == ItemType.Medkit || type == ItemType.SCP500 || type == ItemType.Adrenaline;

        /// <summary>
        /// Check if an <see cref="ItemType">item</see> is a utility item.
        /// </summary>
        /// <param name="type">The item to be checked.</param>
        /// <returns>Returns whether the <see cref="ItemType"/> is an utilty item or not.</returns>
        public static bool IsUtility(this ItemType type) => /*type == ItemType.Disarmer ||*/ type == ItemType.Flashlight || type == ItemType.Radio;

        /// <summary>
        /// Check if a <see cref="ItemType"/> is an armor item.
        /// </summary>
        /// <param name="type">The item to be checked.</param>
        /// <returns>Returns whether the <see cref="ItemType"/> is an armor or not.</returns>
        public static bool IsArmor(this ItemType type) => type == ItemType.ArmorCombat || type == ItemType.ArmorHeavy ||
                                                          type == ItemType.ArmorLight;

        /// <summary>
        /// Check if an <see cref="ItemType">item</see> is a keycard.
        /// </summary>
        /// <param name="type">The item to be checked.</param>
        /// <returns>Returns whether the <see cref="ItemType"/> is a keycard or not.</returns>
        public static bool IsKeycard(this ItemType type) =>
            type == ItemType.KeycardChaosInsurgency || type == ItemType.KeycardContainmentEngineer || type == ItemType.KeycardFacilityManager ||
            type == ItemType.KeycardGuard || type == ItemType.KeycardJanitor || type == ItemType.KeycardNTFCommander ||
            type == ItemType.KeycardNTFLieutenant || type == ItemType.KeycardO5 || type == ItemType.KeycardScientist ||
            type == ItemType.KeycardResearchCoordinator || type == ItemType.KeycardNTFOfficer || type == ItemType.KeycardZoneManager;

        /// <summary>
        /// Gets the default ammo of a weapon.
        /// </summary>
        /// <param name="item">The <see cref="ItemType">item</see> that you want to get durability of.</param>
        /// <returns>Returns the item durability.</returns>
        public static byte GetMaxAmmo(this ItemType item)
        {
            if (!InventoryItemLoader.AvailableItems.TryGetValue(item, out var itemBase) 
                || !(itemBase is InventorySystem.Items.Firearms.Firearm firearm))
                return 0;

            return firearm.AmmoManagerModule.MaxAmmo;
        }

        /// <summary>
        /// Converts a valid ammo <see cref="ItemType"/> into an <see cref="AmmoType"/>.
        /// </summary>
        /// <param name="type">The <see cref="ItemType"/> to convert.</param>
        /// <returns>The ammo type of the given item type.</returns>
        public static AmmoType GetAmmoType(this ItemType type)
        {
            switch (type)
            {
                case ItemType.Ammo9x19:
                    return AmmoType.Ammo9;
                case ItemType.Ammo556x45:
                    return AmmoType.Ammo556;
                case ItemType.Ammo762x39:
                    return AmmoType.Ammo762;
                case ItemType.Ammo12gauge:
                    return AmmoType.Ammo12Gauge;
                case ItemType.Ammo44cal:
                    return AmmoType.Ammo44Cal;
                default:
                    return AmmoType.None;
            }
        }

        /// <summary>
        /// Converts an <see cref="AmmoType"/> into it's corresponding <see cref="ItemType"/>.
        /// </summary>
        /// <param name="type">The <see cref="AmmoType"/> to convert.</param>
        /// <returns>The Item type of the specified ammo.</returns>
        public static ItemType GetItemType(this AmmoType type)
        {
            switch (type)
            {
                case AmmoType.Ammo556:
                    return ItemType.Ammo556x45;
                case AmmoType.Ammo762:
                    return ItemType.Ammo762x39;
                case AmmoType.Ammo9:
                    return ItemType.Ammo9x19;
                case AmmoType.Ammo12Gauge:
                    return ItemType.Ammo12gauge;
                case AmmoType.Ammo44Cal:
                    return ItemType.Ammo44cal;
                default:
                    return ItemType.None;
            }
        }
    }
}
