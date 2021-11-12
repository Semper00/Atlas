using Atlas.EventSystem;
using Atlas.Entities;
using Atlas.Enums;

namespace Atlas.Events
{
    /// <summary>
    /// Fires before the round ends.
    /// </summary>
    public class RoundEnding : BoolEvent
    {
        /// <summary>
        /// Gets the leading team.
        /// </summary>
        public LeadingTeam LeadingTeam { get; }

        /// <summary>
        /// Gets or sets the round summary class list.
        /// </summary>
        public ClassList ClassList { get; set; }

        /// <summary>
        /// Gets or sets the time to restart to the next round.
        /// </summary>
        public int TimeToRestart { get; set; }

        public RoundEnding(LeadingTeam team, ClassList list, int timeToRestart, bool allow)
        {
            LeadingTeam = team;
            ClassList = list;
            TimeToRestart = timeToRestart;
            IsAllowed = allow;
        }
    }
}
