using Atlas.EventSystem;

using System;

namespace Atlas.Events
{
    /// <summary>
    /// Fires when CASSIE announces an NTF entrance.
    /// </summary>
    public class AnnouncingNtfEntrance : BoolEvent
    {
        /// <summary>
        /// Gets or sets the amount of remainig SCPS.
        /// </summary>
        public int ScpsLeft { get; set; }

        /// <summary>
        /// Gets or sets the NTF unit's number.
        /// </summary>
        public int UnitNumber { get; set; }

        /// <summary>
        /// Gets or sets the NTF unit's name.
        /// </summary>
        public string UnitName { get; set; }

        public AnnouncingNtfEntrance(int scps, string name, bool allow)
        {
            ScpsLeft = scps;

            var args = name.Split('-');

            IsAllowed = allow;

            if (args.Length < 2)
                return;

            UnitNumber = int.TryParse(args[1], out int num) ? num : -1;
            UnitName = args[0];
        }

        /// <summary>
        /// Gets the NTF unit's complete name.
        /// </summary>
        /// <returns>The unit's name.</returns>
        public string GetName()
            => UnitName + "-" + UnitNumber;

        /// <summary>
        /// Sets the unit's name. The name <b>must follow this format: NAME-NUMBER</b>
        /// </summary>
        /// <param name="name">The unit's name in the provided format.</param>
        public void SetName(string name)
        {
            if (!name.Contains("-"))
                throw new InvalidOperationException("The NTF's unit name must follow this format: NAME-NUMBER");

            string[] args = name.Split('-');

            if (args.Length != 2)
                throw new InvalidOperationException("The NTF's unit name must follow this format: NAME-NUMBER");

            if (!int.TryParse(args[1], out int number))
                throw new InvalidOperationException("The NTF's unit name must follow this format: NAME-NUMBER");

            UnitName = args[0];
            UnitNumber = number;
        }
    }
}
