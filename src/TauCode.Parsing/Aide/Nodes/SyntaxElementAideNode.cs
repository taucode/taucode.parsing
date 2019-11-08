using System;
using TauCode.Parsing.Aide.Tokens;
using TauCode.Parsing.ParsingUnits.Impl;

namespace TauCode.Parsing.Aide.Nodes
{
    public class SyntaxElementAideNode : ParsingNode
    {
        public SyntaxElementAideNode(SyntaxElement syntaxElement, Action<IToken, IParsingContext> processor)
            : base(processor)
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
