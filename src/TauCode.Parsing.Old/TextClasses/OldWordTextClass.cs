namespace TauCode.Parsing.Old.TextClasses
{
    public sealed class OldWordTextClass : IOldTextClass
    {
        public static OldWordTextClass Instance { get; } = new OldWordTextClass();

        private OldWordTextClass()
        {
        }
    }
}
