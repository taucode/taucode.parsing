namespace TauCode.Parsing.Old.TextClasses
{
    public sealed class OldStringTextClass : IOldTextClass
    {
        public static OldStringTextClass Instance { get; } = new OldStringTextClass();

        private OldStringTextClass()
        {
        }
    }
}
