using System.Reflection;

namespace TauCode.Parsing.Lab
{
    public abstract class TextClassBaseLab : ITextClassLab
    {
        public virtual string Tag => this
            .GetType()
            .GetCustomAttribute<TextClassAttribute>()?.Tag;

        public abstract string TryConvertFrom(string text, ITextClassLab anotherClass);
    }
}
