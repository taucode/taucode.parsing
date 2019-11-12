using System;
using System.Diagnostics;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Units.Impl.Nodes
{
    [DebuggerDisplay("<Identifier>")]
    public class IdentifierNode : ProcessingNode
    {
        public IdentifierNode(Action<IToken, IContext> processor, string name)
            : base(processor, name)
        {
        }

        protected override bool IsAcceptableToken(IToken token)
        {
            return token is WordToken;
        }
    }
}
