using System;
using TauCode.Parsing.Aide.Tokens;
using TauCode.Parsing.ParsingUnits.Impl;

namespace TauCode.Parsing.Aide.Nodes
{
    public class NameReferenceAideNode : ParsingNode
    {
        public NameReferenceAideNode(Action<IToken, IParsingContext> processor)
            : base(processor)
        {
        }

        protected override bool IsAcceptableToken(IToken token)
        {
            return token is NameReferenceAideToken;
        }
    }
}
