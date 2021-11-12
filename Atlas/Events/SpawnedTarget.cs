using Atlas.Entities;
using Atlas.EventSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Fires when a target spawns.
    /// </summary>
    public class SpawnedTarget : BoolEvent
    {
        /// <summary>
        /// Gets the target that spawned.
        /// </summary>
        public Target Target { get; }

        public SpawnedTarget(Target target)
            => Target = target;
    }
}
