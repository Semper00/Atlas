using Atlas.Entities;
using Atlas.Enums;
using Atlas.EventSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Fires before SCP-914 changes it's knob setting.
    /// </summary>
    public class ChangingKnob : BoolEvent
    {
        /// <summary>
        /// Gets the player that is changing the knob's status.
        /// </summary>
        public Player Player { get; }
        
        /// <summary>
        /// Gets the current knob setting.
        /// </summary>
        public KnobSetting Current { get; }

        /// <summary>
        /// Gets or sets the new knob setting.
        /// </summary>
        public KnobSetting New { get; set; }

        public ChangingKnob(Player player, KnobSetting cur, KnobSetting n, bool allow)
        {
            Player = player;
            Current = cur;
            New = n;
            IsAllowed = allow;
        }
    }
}
