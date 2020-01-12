namespace TauCode.Parsing.Old.TextClasses
{
    public class OldIdentifierTextClass : IOldTextClass
    {
        public static OldIdentifierTextClass Instance { get; } = new OldIdentifierTextClass();

        private OldIdentifierTextClass()
        {   
        }
    }
}
