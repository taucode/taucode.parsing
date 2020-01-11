namespace TauCode.Parsing.TextClasses
{
    public class WordTextClass : ITextClass
    {
        public static readonly WordTextClass Instance = new WordTextClass();
        
        private WordTextClass()
        {   
        }
    }
}
