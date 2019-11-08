using System;
using TauCode.Parsing.Aide.Tokens;
using TauCode.Parsing.Units.Impl;

namespace TauCode.Parsing.Aide.Nodes
{
    public class WordAideNode : Node
    {
        public WordAideNode(Action<IToken, IContext> processor)
            : base(processor)
        {
        }

        protected override bool IsAcceptableToken(IToken token)
        {
            return token is WordAideToken;
        }
    }
}
