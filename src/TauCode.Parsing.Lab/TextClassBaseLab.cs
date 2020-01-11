using System;
using System.Reflection;

namespace TauCode.Parsing.Lab
{
    public abstract class TextClassBaseLab : ITextClassLab
    {
        public virtual string Tag => this
            .GetType()
            .GetCustomAttribute<TextClassAttribute>()?.Tag;

        public string TryConvertFrom(string text, ITextClassLab anotherClass)
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

        protected abstract string TryConvertFromImpl(string text, ITextClassLab anotherClass);
    }
}
