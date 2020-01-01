namespace TauCode.Parsing.Tokens.TextClasses
{
    public class WordTextClass : ITextClass
    {
        public static readonly WordTextClass Instance = new WordTextClass();
        
        private WordTextClass()
        {   
        }
    }
}
