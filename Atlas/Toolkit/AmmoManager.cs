using System;
using System.Collections.Generic;
using System.Linq;

using Atlas.Entities;
using Atlas.Enums;
using Atlas.Extensions;

using InventorySystem;
using InventorySystem.Items.Firearms;

namespace Atlas.Toolkit
{
    /// <summary>
    /// This class is used to manage ammo of a player.
    /// </summary>
    public class AmmoManager
    {
        private Player _p;

        private static HashSet<AmmoType> ammoTypes;

        private AmmoManager(Player p)
        {
            if (ammoTypes == null)
            {
                ammoTypes = Enum.GetValues(typeof(AmmoType)).Cast<AmmoType>().ToHashSet();
            }

            _p = p;
        }

        /// <summary>
        /// Gets the player's ammo box.
        /// </summary>
        public Dictionary<ItemType, ushort> Ammo { get => _p.Inventory.UserInventory.ReserveAmmo; }

        /// <summary>
        /// Gets or sets the player's total ammo (in inventory).
        /// </summary>
        public ushort TotalAmmo
        {
            get
            {
                ushort totalAmmo = 0;

                foreach (var pair in Ammo)
                    totalAmmo += pair.Value;

                return totalAmmo;
            }
            set
            {
                foreach (var pair in Ammo)
                    Ammo[pair.Key] = value;

                _p.SendItems();
            }
        }

        /// <summary>
        /// Gets or sets the player's ammo in his current firearm.
        /// </summary>
        public byte AmmoInFirearm
        {
            get
            {
                if (_p.Inventory.CurInstance is Firearm firearm)
                {
                    return firearm.Status.Ammo;
                }
                else
                    return unchecked((byte)-1);
            }
            set
            {
                if (_p.Inventory.CurInstance is Firearm firearm)
                {
                    firearm.Status = new FirearmStatus(value, firearm.Status.Flags, firearm.Status.Attachments);
                }
            }
        }

        /// <summary>
        /// Gets or sets the ammo in all firearms.
        /// </summary>
        public byte AmmoInAllFirearms
        {
            get
            {
                byte curAmmo = 0;

                foreach (Firearm firearm in _p.GetItems<Firearm>())
                {
                    curAmmo += firearm.Status.Ammo;
                }

                return curAmmo;
            }
            set
            {
                foreach (Firearm firearm in _p.GetItems<Firearm>())
                {
                    firearm.Status = new FirearmStatus(value, firearm.Status.Flags, firearm.Status.Attachments);
                }
            }
        }

        /// <summary>
        /// Adds the specified amount of ammo to the player's inventory.
        /// </summary>
        /// <param name="ammoType">The ammo type to add.</param>
        /// <param name="amount">The amount of ammo to add.</param>
        public void AddAmmo(AmmoType ammoType, ushort amount)
        {
            Ammo[ammoType.GetItemType()] = (ushort)(Ammo[ammoType.GetItemType()] + amount);

            _p.SendItems();
        }

        /// <summary>
        /// Removes all ammo of this type.
        /// </summary>
        /// <param name="ammoType">The ammo type to remove.</param>
        public void RemoveAmmo(AmmoType ammoType)
        {
            Ammo[ammoType.GetItemType()] = 0;

            _p.SendItems();
        }

        /// <summary>
        /// Removes the specified amount of ammo.
        /// </summary>
        /// <param name="ammoType">The ammo type to remove.</param>
        /// <param name="amount">The ammo amount to remove.</param>
        public void RemoveAmmo(AmmoType ammoType, ushort amount)
        {
            Ammo[ammoType.GetItemType()] -= amount;

            _p.SendItems();
        }

        /// <summary>
        /// Removes all ammo.
        /// </summary>
        public void RemoveAmmo()
        {
            foreach (AmmoType ammoType in ammoTypes)
                RemoveAmmo(ammoType);
        }

        /// <summary>
        /// Drops all of the specified ammo type.
        /// </summary>
        /// <param name="ammoType">The ammo type to drop.</param>
        public void DropAmmo(AmmoType ammoType)
        {
            _p.Inventory.ServerDropAmmo(ammoType.GetItemType(), GetAmmo(ammoType));
        }

        /// <summary>
        /// Drops all ammo.
        /// </summary>
        public void DropAmmo()
        {
            foreach (AmmoType type in ammoTypes)
                DropAmmo(type);
        }

        /// <summary>
        /// Drops the specified amount of ammo type.
        /// </summary>
        /// <param name="ammoType">The ammo type to drop.</param>
        /// <param name="amount">The amount of ammo to drop.</param>
        public void DropAmmo(AmmoType ammoType, ushort amount)
        {
            _p.Inventory.ServerDropAmmo(ammoType.GetItemType(), amount);
        }

        /// <summary>
        /// Gets the amount of the specified ammo.
        /// </summary>
        /// <param name="ammoType">The ammo type to retrieve.</param>
        /// <returns>The amount of the specified ammo type.</returns>
        public ushort GetAmmo(AmmoType ammoType)
            => Ammo[ammoType.GetItemType()];

        /// <summary>
        /// Gets a new <see cref="AmmoManager"/> instance for the specified player.
        /// </summary>
        /// <param name="player">The player to get an <see cref="AmmoManager"/> for.</param>
        /// <returns>The <see cref="AmmoManager"/> instance.</returns>
        public static AmmoManager GetAmmoManager(Player player)
            => new AmmoManager(player);
    }
}
