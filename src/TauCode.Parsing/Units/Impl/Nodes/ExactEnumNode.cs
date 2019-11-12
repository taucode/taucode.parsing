using System;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Units.Impl.Nodes
{
    public class ExactEnumNode<TEnum> : ProcessingNode where TEnum : struct
    {
        public ExactEnumNode(TEnum value, Action<IToken, IContext> processor, string name)
            : base(processor, name)
        {
            this.Value = value;
        }

        public TEnum Value { get; }

        protected override bool IsAcceptableToken(IToken token)
        {
            return
                token is EnumToken<TEnum> enumToken &&
                enumToken.Value.Equals(this.Value);
        }
    }
}
