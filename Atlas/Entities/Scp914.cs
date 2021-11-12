using Scp914;

using Atlas.Enums;

using UnityEngine;

using System.Linq;

namespace Atlas.Entities
{
    /// <summary>
    /// A class for SCP-914 management.
    /// </summary>
    public class Scp914
    {
        internal Scp914Controller ctrl;

        internal Scp914(Scp914Controller ctrl)
        {
            this.ctrl = ctrl;

            IntakeDoor = Door.Get(ctrl._doors.FirstOrDefault());
            OutputDoor = Door.Get(ctrl._doors.LastOrDefault());
        }

        /// <summary>
        /// Gets or sets the SCP-914's knob setting.
        /// </summary>
        public KnobSetting Setting { get => (KnobSetting)ctrl.Network_knobSetting; set => ctrl.Network_knobSetting = (Scp914KnobSetting)value; }

        /// <summary>
        /// Gets the next SCP-914 knob setting.
        /// </summary>
        public KnobSetting NewSetting
        {
            get
            {
                switch (Setting)
                {
                    case KnobSetting.Rough:
                        return KnobSetting.Coarse;
                    case KnobSetting.Coarse:
                        return KnobSetting.OneToOne;
                    case KnobSetting.OneToOne:
                        return KnobSetting.Fine;
                    case KnobSetting.Fine:
                        return KnobSetting.VeryFine;
                    case KnobSetting.VeryFine:
                        return KnobSetting.Rough;
                    default:
                        return KnobSetting.Rough;
                }
            }
        }

        /// <summary>
        /// Gets or sets the intake's <see cref="Transform"/>.
        /// </summary>
        public Transform Intake { get => ctrl._intakeChamber; set => ctrl._intakeChamber = value; }

        /// <summary>
        /// Gets or sets the output's <see cref="Transform"/>.
        /// </summary>
        public Transform Output { get => ctrl._outputChamber; set => ctrl._outputChamber = value; }

        /// <summary>
        /// Gets or sets the size of a chamber.
        /// </summary>
        public float ChamberSize { get => ctrl._chamberSize; set => ctrl._chamberSize = value; }

        /// <summary>
        /// Gets or sets the SCP-914's remaining cooldown.
        /// </summary>
        public float Cooldown { get => ctrl._remainingCooldown; set => ctrl._remainingCooldown = value; }

        /// <summary>
        /// Gets or sets the SCP-914's remainig cooldown for a knob change.
        /// </summary>
        public float KnobChangeCooldown { get => ctrl._knobChangeCooldown; set => ctrl._knobChangeCooldown = value; }

        /// <summary>
        /// Gets a value indicating whether SCP-914 is currently upgrading or not.
        /// </summary>
        public bool IsUpgrading { get => ctrl._isUpgrading; }

        /// <summary>
        /// Gets or sets the mode of SCP-914.
        /// </summary>
        public Enums.Scp914Mode Mode
        {
            get => (Enums.Scp914Mode)ctrl._configMode.Value;
            set
            {
                ctrl._configMode.Value = (global::Scp914.Scp914Mode)value;

                GameCore.ConfigFile.ServerConfig.SetString(ctrl._configMode.Key, ctrl._configMode.Value.ToString());
                GameCore.ConfigFile.ReloadGameConfigs();
            }
        }

        /// <summary>
        /// Gets the door of the intake chamber.
        /// </summary>
        public Door IntakeDoor { get; }

        /// <summary>
        /// Gets the door of the output chamber.
        /// </summary>
        public Door OutputDoor { get; }

        /// <summary>
        /// Plays a sound.
        /// </summary>
        /// <param name="type">The sound to play.</param>
        public void PlaySound(Scp914SoundType type)
            => ctrl.RpcPlaySound((byte)type);
    }
}
