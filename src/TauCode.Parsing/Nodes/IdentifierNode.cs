using System;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Nodes
{
    public class IdentifierNode : ActionNode
    {
        public IdentifierNode(INodeFamily family, string name, Action<IToken, IResultAccumulator> action)
            : base(family, name, action)
        {
        }

        protected override InquireResult InquireImpl(IToken token, IResultAccumulator resultAccumulator)
        {
            if (token is IdentifierToken)
            {
                return this.Action == null ? InquireResult.Skip : InquireResult.Act;
            }
            else
            {
                return InquireResult.Reject;
            }
        }
    }
}
