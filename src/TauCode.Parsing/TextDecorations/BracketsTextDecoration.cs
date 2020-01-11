namespace TauCode.Parsing.TextDecorations
{
    public class BracketsTextDecoration : ITextDecoration
    {
        public static readonly BracketsTextDecoration Instance = new BracketsTextDecoration();

        private BracketsTextDecoration()
        {
        }
    }
}
