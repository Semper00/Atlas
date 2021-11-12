using Atlas.EventSystem;
using Atlas.Entities;
using Atlas.Enums;

namespace Atlas.Events
{
    /// <summary>
    /// Fires before a player interacts with a generator.
    /// </summary>
    public class InteractingGenerator : BoolEvent
    {
        /// <summary>
        /// Gets the player.
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the generator.
        /// </summary>
        public Generator Generator { get; }

        /// <summary>
        /// Gets the generator operation.
        /// </summary>
        public GeneratorAction Operation { get; }

        public InteractingGenerator(Player player, Generator generator, GeneratorAction op, bool allow)
        {
            Player = player;
            Generator = generator;
            Operation = op;
            IsAllowed = allow;
        }
    }
}