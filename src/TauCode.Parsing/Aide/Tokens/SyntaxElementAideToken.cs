using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Aide.Tokens
{
    public class SyntaxElementAideToken : EnumToken<SyntaxElement>
    {
        public SyntaxElementAideToken(SyntaxElement syntaxElement, string name)
            : base(syntaxElement, name)
        {
            this.SyntaxElement = syntaxElement;
        }

        public SyntaxElement SyntaxElement { get; }
    }
}
