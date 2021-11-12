using Atlas.EventSystem;
using Atlas.EventSystem.Attributes;

namespace Atlas.Events
{
    /// <summary>
    /// Fires before the server's process exits.
    /// </summary>
    [NoParam]
    public class ServerQuitting : Event
    {
    }
}
