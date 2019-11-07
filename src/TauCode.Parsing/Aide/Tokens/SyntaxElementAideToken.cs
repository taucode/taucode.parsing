namespace TauCode.Parsing.Aide.Tokens
{
    public class SyntaxElementAideToken : AideToken
    {
        public SyntaxElementAideToken(SyntaxElement syntaxElement, string name)
            : base(name)
        {
            this.SyntaxElement = syntaxElement;
        }

        public SyntaxElement SyntaxElement { get; }
    }
}
