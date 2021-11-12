using System.ComponentModel;

using Atlas.Interfaces;

namespace Atlas.ModManager.Configs.Atlas
{
    public class Scp914Configuration : IConfig
    {
        public float KnobChangeCooldown { get; set; } = 1f;

        public bool PlaySoundOnKnobChange { get; set; } = true;

        public bool PlaySoundOnActivation { get; set; } = true;
    }
}
