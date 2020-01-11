namespace TauCode.Parsing.TextClasses
{
    [TextClass("word")]
    public class WordTextClass : TextClassBase
    {
        public static WordTextClass Instance { get; } = new WordTextClass();

        private WordTextClass()
        {   
        }

        protected override string TryConvertFromImpl(string text, ITextClass anotherClass) => null;
    }
}
