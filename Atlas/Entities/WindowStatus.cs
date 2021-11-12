using UnityEngine;

namespace Atlas.Entities
{
    /// <summary>
    /// A struct to simplify setting the breakble windows status.
    /// </summary>
    public struct WindowStatus
    {
        /// <summary>
        /// Initialzes a new instance of the <see cref="WindowStatus"/> struct.
        /// </summary>
        /// <param name="orig">The original <see cref="BreakableWindow.BreakableWindowStatus"/>.</param>
        public WindowStatus(BreakableWindow.BreakableWindowStatus orig)
        {
            IsBroken = orig.broken;
            Position = orig.position;
            Rotation = orig.rotation;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this window is broken or not.
        /// </summary>
        public bool IsBroken { get; set; }

        /// <summary>
        /// Gets or sets the window's position.
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Gets or sets the window's rotation.
        /// </summary>
        public Quaternion Rotation { get; set; }

        /// <summary>
        /// Converts this instance to <see cref="BreakableWindow.BreakableWindowStatus"/>.
        /// </summary>
        /// <returns>The converted instance.</returns>
        public BreakableWindow.BreakableWindowStatus ToBase()
            => new BreakableWindow.BreakableWindowStatus
            {
                broken = IsBroken,
                position = Position,
                rotation = Rotation
            };
    }
}