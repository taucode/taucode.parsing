namespace TauCode.Parsing.TextClasses
{
    public class IdentifierTextClass : ITextClass
    {
        public static readonly IdentifierTextClass Instance = new IdentifierTextClass();
        
        private IdentifierTextClass()
        {   
        }
    }
}
