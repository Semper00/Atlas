using Atlas.EventSystem;

namespace Atlas.Events
{
    /// <summary>
    /// Fires before CASSIE announces a new message.
    /// </summary>
    public class SendingCassieMessage : BoolEvent
    {
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Words { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the message should be held.
        /// </summary>
        public bool MakeHold { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the message should make noise.
        /// </summary>
        public bool MakeNoise { get; set; }

        public SendingCassieMessage(string words, bool makeHold, bool makeNoise, bool allow)
        {
            Words = words;
            MakeHold = makeHold;
            MakeNoise = makeNoise;
            IsAllowed = allow;
        }
    }
}
