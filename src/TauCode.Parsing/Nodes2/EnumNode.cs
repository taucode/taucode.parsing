using System;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Nodes2
{
    public class EnumNode<TEnum> : ActionNode where TEnum : struct
    {
        public EnumNode(INodeFamily family, string name, Action<IToken, IResultAccumulator> action)
            : base(family, name, action)
        {
        }

        protected override InquireResult InquireImpl(IToken token)
        {
            if (token is EnumToken<TEnum>)
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
