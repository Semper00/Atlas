﻿namespace Atlas.Enums
{
    /// <summary>
    /// Status effects as enum.
    /// </summary>
    public enum EffectType
    {
        /// <summary>
        /// The player isn't able to open their inventory or reload a weapon.
        /// </summary>
        Amnesia,

        /// <summary>
        /// Drains the player's stamina and then health.
        /// </summary>
        Asphyxiated,

        /// <summary>
        /// Damages the player over time.
        /// </summary>
        Bleeding,

        /// <summary>
        /// Blurs the player's screen.
        /// </summary>
        Blinded,

        /// <summary>
        /// Increases damage the player gets.
        /// </summary>
        Burned,

        /// <summary>
        /// Blurs the player's screen when rotating.
        /// </summary>
        Concussed,

        /// <summary>
        /// Teleports the player to the pocket dimension and drains health.
        /// </summary>
        Corroding,

        /// <summary>
        /// Deafens the player.
        /// </summary>
        Deafened,

        /// <summary>
        /// Removes 10% of the player's health per second.
        /// </summary>
        Decontaminating,

        /// <summary>
        /// Slows down the player's movement.
        /// </summary>
        Disabled,

        /// <summary>
        /// Stops the player's movement.
        /// </summary>
        Ensnared,

        /// <summary>
        /// Halves the player's maximum stamina and stamina regeneration rate.
        /// </summary>
        Exhausted,

        /// <summary>
        /// Flashes the player.
        /// </summary>
        Flashed,

        /// <summary>
        /// Drains the player's health while sprinting.
        /// </summary>
        Hemorrhage,

        /// <summary>
        /// Reduces the player's FOV, gives infinite stamina and gives the effect of underwater sound.
        /// </summary>
        Invigorated,

        /// <summary>
        /// Increases the player's stamina consumption.
        /// </summary>
        Panic,

        /// <summary>
        /// Damages the player every 5 seconds, starting low and ramping hight.
        /// </summary>
        Poisoned,

        /// <summary>
        /// Makes the player faster but also drains health.
        /// </summary>
        Scp207,

        /// <summary>
        /// Makes the player invisibility.
        /// </summary>
        Scp268,

        /// <summary>
        /// Slows down the player's movement with SCP-106 effect.
        /// </summary>
        SinkHole,

        /// <summary>
        /// Gives the player the sound vision of SCP-939.
        /// </summary>
        Visuals939,
    }
}
