using Atlas.EventSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Fires when CASSIE announces decontamination.
    /// </summary>
    public class AnnouncingDecontamination : Event
    {
        /// <summary>
        /// Gets or sets the announcement ID (<b>from 0 to 6</b>).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the announcement is going to be global or not.
        /// </summary>
        public bool Global { get; set; }

        public AnnouncingDecontamination(int id, bool global)
        {
            Id = id;
            Global = global;
        }
    }
}
