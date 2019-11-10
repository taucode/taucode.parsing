using System;
using TauCode.Parsing.Aide.Tokens;
using TauCode.Parsing.Units.Impl.Nodes;

namespace TauCode.Parsing.Aide.Nodes
{
    public class NameReferenceAideNode : ProcessingNode
    {
        private NameReferenceAideNode(Action<IToken, IContext> processor)
            : base(processor, null)
        {
        }

        public NameReferenceAideNode(Action<IToken, IContext> processor, string name)
            : base(processor, name)
        {
        }

        protected override bool IsAcceptableToken(IToken token)
        {
            return token is NameReferenceAideToken;
        }
    }
}
