using System;
using System.Diagnostics;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Units.Impl.Nodes
{
    [DebuggerDisplay("<Identifier>")]
    public class IdentifierNode : Node
    {
        public IdentifierNode(Action<IToken, IContext> processor)
            : base(processor)
        {
        }

        protected override bool IsAcceptableToken(IToken token)
        {
            return token is WordToken;
        }
    }
}
