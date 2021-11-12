using System;

namespace Atlas.Exceptions
{
    /// <summary>
    /// Occurs when a ReferenceHub happens to be null.
    /// </summary>
    public class HubNotFoundException : Exception
    {
        internal HubNotFoundException(string argument = "ReferenceHub")
            : base($"The ReferenceHub cannot be null! Parameter name: {argument}") { }
    }
}
