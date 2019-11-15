using System;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Nodes2
{
    public class WordNode : ActionNode
    {
        public WordNode(INodeFamily family, string name, Action<IToken, IResultAccumulator> action)
            : base(family, name, action)
        {
        }

        protected override InquireResult InquireImpl(IToken token)
        {
            if (token is WordToken)
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
