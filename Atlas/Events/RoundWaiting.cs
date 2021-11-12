using Atlas.EventSystem;
using Atlas.EventSystem.Attributes;

namespace Atlas.Events
{
    /// <summary>
    /// Fires when the server starts waiting for players.
    /// </summary>
    [NoParam]
    public class RoundWaiting : Event
    {
    }
}
