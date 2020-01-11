using System;

namespace TauCode.Parsing.Lab.TextClasses
{
    public class StringTextClassLab : ITextClassLab
    {
        public static StringTextClassLab Instance { get; } = new StringTextClassLab();

        private StringTextClassLab()
        {   
        }

        public string TryConvertFrom(string text, ITextClassLab anotherClass)
        {
            throw new NotImplementedException();
        }
    }
}
