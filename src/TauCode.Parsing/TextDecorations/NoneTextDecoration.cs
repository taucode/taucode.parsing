namespace TauCode.Parsing.Tokens.TextDecorations
{
    public class NoneTextDecoration : ITextDecoration
    {
        public static readonly NoneTextDecoration Instance = new NoneTextDecoration();

        private NoneTextDecoration()
        {
        }
    }
}
