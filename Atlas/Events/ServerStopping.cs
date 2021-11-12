using Atlas.EventSystem.Attributes;
using Atlas.EventSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Fires before the server process quits. This event may not fire every time.
    /// </summary>
    [NoParam]
    public class ServerStopping : Event
    {
    }
}
