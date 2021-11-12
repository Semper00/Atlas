using Atlas.Entities;
using Atlas.EventSystem;

using InventorySystem.Items.Firearms.BasicMessages;

using UnityEngine;

namespace Atlas.Events
{
    public class Shooting : BoolEvent
    {
        /// <summary>
        /// Gets the player who's shooting.
        /// </summary>
        public Player Shooter { get; }

        /// <summary>
        /// Gets or sets the <see cref="ShotMessage"/> for the event.
        /// </summary>
        public ShotMessage ShotMessage { get; set; }

        /// <summary>
        /// Gets or sets the position of the shot.
        /// </summary>
        public Vector3 ShotPosition
        {
            get => ShotMessage.TargetPosition;
            set
            {
                ShotMessage msg = ShotMessage;

                msg.TargetPosition = value;

                ShotMessage = msg;
            }
        }

        /// <summary>
        /// Gets or sets the nedId of the target of the shot.
        /// </summary>
        public uint TargetNetId
        {
            get => ShotMessage.TargetNetId;
            set
            {
                ShotMessage msg = ShotMessage;

                msg.TargetNetId = value;

                ShotMessage = msg;
            }
        }

        public Shooting(Player shooter, ShotMessage msg)
        {
            Shooter = shooter;
            ShotMessage = msg;
        }
    }
}
