﻿using System;

using Atlas.Entities;

using CustomPlayerEffects;

namespace Atlas.Events
{
    public class ReceivingEffectEventArgs : EventArgs
    {
        private byte state;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReceivingEffectEventArgs"/> class.
        /// </summary>
        /// <param name="player">The <see cref="Player"/> receiving the effect.</param>
        /// <param name="effect">The <see cref="PlayerEffect"/> being added to the player.</param>
        /// <param name="state">The state the effect is being changed to.</param>
        /// <param name="currentState">The current state of the effect being changed.</param>
        public ReceivingEffectEventArgs(Player player, PlayerEffect effect, byte state, byte currentState)
        {
            Player = player;
            Effect = effect;
            this.state = state;
            CurrentState = currentState;
        }

        /// <summary>
        /// Gets the <see cref="Player"/> receiving the effect.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the <see cref="PlayerEffect"/> being received.
        /// </summary>
        public PlayerEffect Effect { get; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the effect will be applied.
        /// </summary>
        public bool IsAllowed { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating how long the effect will last.
        /// </summary>
        public float Duration { get; set; } = 0.0f;

        /// <summary>
        /// Gets or sets the value of the new state of the effect. Setting this to 0 is the same as setting IsAllowed to false.
        /// </summary>
        public byte State
        {
            get => state;
            set
            {
                state = value;

                if (state == 0)
                    IsAllowed = false;
            }
        }

        /// <summary>
        /// Gets the value of the current state of this effect on the player.
        /// </summary>
        public byte CurrentState { get; }
    }
}
