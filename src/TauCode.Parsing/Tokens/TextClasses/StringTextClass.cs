namespace TauCode.Parsing.Tokens.TextClasses
{
    public class StringTextClass : ITextClass
    {
        public static readonly StringTextClass Instance = new StringTextClass();
        
        private StringTextClass()
        {   
        }
    }
}
