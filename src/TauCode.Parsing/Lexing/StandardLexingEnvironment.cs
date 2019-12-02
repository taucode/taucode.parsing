namespace TauCode.Parsing.Lexing
{
    public class StandardLexingEnvironment : ILexingEnvironment
    {
        public static ILexingEnvironment Instance { get; } = new StandardLexingEnvironment();

        private StandardLexingEnvironment()
        {   
        }

        public bool IsSpace(char c) => LexingHelper.IsSpace(c);

        public bool IsLineBreak(char c) => LexingHelper.IsLineBreak(c);
    }
}
