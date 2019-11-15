using System;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Nodes2
{
    public class ExactEnumNode<TEnum> : ActionNode where TEnum : struct
    {
        public ExactEnumNode(INodeFamily family, string name, Action<IToken, IResultAccumulator> action, TEnum value)
            : base(family, name, action)
        {
            this.Value = value;
        }

        public TEnum Value { get; }

        protected override InquireResult InquireImpl(IToken token, IResultAccumulator resultAccumulator)
        {
            if (token is EnumToken<TEnum> enumToken && enumToken.Value.Equals(this.Value))
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
