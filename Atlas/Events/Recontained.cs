using Atlas.Entities;
using Atlas.EventSystem;

namespace Atlas.Events
{
    public class Recontained : Event
    {
        public Recontained(Player target)
        {
            Target = target;
        }

        /// <summary>
        /// Gets the player that previously controlled SCP-079.
        /// </summary>
        public Player Target { get; }
    }
}
