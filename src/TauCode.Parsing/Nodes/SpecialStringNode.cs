using System;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Nodes
{
    public class SpecialStringNode : ActionNode
    {
        public SpecialStringNode(
            INodeFamily family,
            string name,
            Action<IToken, IResultAccumulator> action,
            string @class)
            : base(family, name, action)
        {
            this.Class = @class ?? throw new ArgumentNullException(nameof(@class));
        }

        public string Class { get; }

        protected override InquireResult InquireImpl(IToken token, IResultAccumulator resultAccumulator)
        {
            if (
                token is SpecialStringToken specialStringToken &&
                specialStringToken.Class.Equals(this.Class))
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
