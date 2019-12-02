using System;
using TauCode.Parsing.Lexizing;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Nodes
{
    public class ExactPunctuationNode : ActionNode
    {
        public ExactPunctuationNode(
            char c,
            Action<IToken, IResultAccumulator> action,
            INodeFamily family,
            string name) : base(action, family, name)
        {
            if (!LexizingHelper.IsStandardPunctuationChar(c))
            {
                throw new ArgumentOutOfRangeException(nameof(c));
            }

            this.Value = c;
        }

        public char Value { get; }

        protected override InquireResult InquireImpl(IToken token, IResultAccumulator resultAccumulator)
        {
            if (token is PunctuationToken punctuationToken && punctuationToken.Value.Equals(this.Value))
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
