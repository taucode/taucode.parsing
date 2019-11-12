using System;
using TauCode.Parsing.Aide.Tokens;
using TauCode.Parsing.Units.Impl.Nodes;

namespace TauCode.Parsing.Aide.Nodes
{
    public class SyntaxElementAideNode : ProcessingNode
    {
        public SyntaxElementAideNode(SyntaxElement syntaxElement, Action<IToken, IContext> processor, string name)
            : base(processor, name)
        {
            this.SyntaxElement = syntaxElement;
        }

        protected override bool IsAcceptableToken(IToken token)
        {
            var result =
                token is SyntaxElementAideToken syntaxElementAideToken &&
                syntaxElementAideToken.SyntaxElement == this.SyntaxElement;

            return result;
        }

        public SyntaxElement SyntaxElement { get; }
    }
}
