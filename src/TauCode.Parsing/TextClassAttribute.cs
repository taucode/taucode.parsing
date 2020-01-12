using System;

namespace TauCode.Parsing
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class TextClassAttribute : Attribute
    {
        public TextClassAttribute(string tag)
        {
            this.Tag = tag ?? throw new ArgumentNullException(nameof(tag));
        }
        public string Tag { get; }
    }
}
