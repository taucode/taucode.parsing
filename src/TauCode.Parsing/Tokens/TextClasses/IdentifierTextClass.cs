namespace TauCode.Parsing.Tokens.TextClasses
{
    public class IdentifierTextClass : ITextClass
    {
        public static readonly IdentifierTextClass Instance = new IdentifierTextClass();
        
        private IdentifierTextClass()
        {   
        }
    }
}
