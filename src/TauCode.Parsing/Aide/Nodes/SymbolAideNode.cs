using System;
using TauCode.Parsing.Aide.Tokens;
using TauCode.Parsing.Units.Impl;

namespace TauCode.Parsing.Aide.Nodes
{
    public class SymbolAideNode : Node
    {
        public SymbolAideNode(Action<IToken, IContext> processor)
            : base(processor)
        {
        }

        protected override bool IsAcceptableToken(IToken token)
        {
            return token is SymbolAideToken;
        }
    }
}
