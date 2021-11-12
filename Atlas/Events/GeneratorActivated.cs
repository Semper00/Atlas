using Atlas.Entities;
using Atlas.EventSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Fires when a generator activates.
    /// </summary>
    public class GeneratorActivated : BoolEvent
    {
        /// <summary>
        /// Gets the generator that activated.
        /// </summary>
        public Generator Generator { get; }

        public GeneratorActivated(Generator gen, bool allow)
        {
            Generator = gen;
            IsAllowed = allow;
        }
    }
}
