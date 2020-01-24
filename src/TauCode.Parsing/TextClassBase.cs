using System;
using System.Reflection;

namespace TauCode.Parsing
{
    public abstract class TextClassBase : ITextClass
    {
        public virtual string Tag => this
            .GetType()
            .GetCustomAttribute<TextClassAttribute>()?.Tag;

        public string TryConvertFrom(string text, ITextClass anotherClass)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (anotherClass == null)
            {
                throw new ArgumentNullException(nameof(anotherClass));
            }

            if (anotherClass.GetType() == this.GetType())
            {
                return text;
            }

            return this.TryConvertFromImpl(text, anotherClass);
        }

        protected abstract string TryConvertFromImpl(string text, ITextClass anotherClass);
    }
}
