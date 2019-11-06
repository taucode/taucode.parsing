using System;
using System.Diagnostics;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.ParsingUnits.Impl.Nodes
{
    [DebuggerDisplay("<Identifier>")]
    public class IdentifierNode : ParsingNode
    {
        public IdentifierNode(Action<IToken, IParsingContext> processor)
            : base(processor)
        {
        }

        protected override bool IsAcceptableToken(IToken token)
        {
            return token is WordToken;
        }
    }
}
