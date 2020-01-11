namespace TauCode.Parsing.Lab.TextClasses
{
    [TextClass("word")]
    public class WordTextClassLab : TextClassBaseLab
    {
        public static WordTextClassLab Instance { get; } = new WordTextClassLab();

        private WordTextClassLab()
        {   
        }

        public override string TryConvertFrom(string text, ITextClassLab anotherClass) => null;
    }
}
