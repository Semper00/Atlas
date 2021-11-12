using Atlas.EventSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Fires before Light Containment Zone decontamination starts.
    /// </summary>
    public class LczDecontaminating : BoolEvent
    {
        public LczDecontaminating(bool allow)
            => IsAllowed = allow;
    }
}
