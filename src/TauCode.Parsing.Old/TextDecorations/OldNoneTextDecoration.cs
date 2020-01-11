namespace TauCode.Parsing.Old.TextDecorations
{
    public class OldNoneTextDecoration : IOldTextDecoration
    {
        public static readonly OldNoneTextDecoration Instance = new OldNoneTextDecoration();

        private OldNoneTextDecoration()
        {
        }
    }
}
