using System;

namespace Atlas.Commands.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class CommandAttribute : Attribute
    {
        public string Text { get; }

        public bool? IgnoreExtraArgs { get; }

        public CommandAttribute()
        {
            Text = null;
        }

        public CommandAttribute(string text)
        {
            Text = text;
        }

        public CommandAttribute(string text, bool ignoreExtraArgs)
        {
            Text = text;
            IgnoreExtraArgs = ignoreExtraArgs;
        }
    }
}
