namespace TauCode.Parsing.TextDecorations
{
    public class NoneTextDecoration : ITextDecoration
    {
        public static NoneTextDecoration Instance { get; } = new NoneTextDecoration();
        private NoneTextDecoration()
        {
        }
    }
}
