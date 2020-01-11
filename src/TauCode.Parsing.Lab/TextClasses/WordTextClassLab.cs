using System;

namespace TauCode.Parsing.Lab.TextClasses
{
    public class WordTextClassLab : ITextClassLab
    {
        public static WordTextClassLab Instance { get; } = new WordTextClassLab();

        private WordTextClassLab()
        {   
        }

        public string TryConvertFrom(string text, ITextClassLab anotherClass)
        {
            throw new NotImplementedException();
        }
    }
}
