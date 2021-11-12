using Atlas.EventSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Fires before CASSIE sends an announcement.
    /// </summary>
    public class Announcing : BoolEvent
    {
        /// <summary>
        /// Gets or sets the announcement text.
        /// </summary>
        public string Words { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the announcement is held or not.
        /// </summary>
        public bool IsHeld { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the announcement should be noisy or not.
        /// </summary>
        public bool MakeNoise { get; set; }

        public Announcing(string ann, bool held, bool noise, bool allow)
        {
            Words = ann;
            IsHeld = held;
            MakeNoise = noise;
            IsAllowed = allow;
        }
    }
}
