using System;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Tokens;

// todo clean
namespace TauCode.Parsing.Nodes
{
    public class ExactPunctuationNode : ActionNode
    {
        public ExactPunctuationNode(
            char c,
            Action<ActionNode, IToken, IResultAccumulator> action,
            INodeFamily family,
            string name)
            : base(action, family, name)
        {
            if (!LexingHelper.IsStandardPunctuationChar(c))
            {
                throw new ArgumentOutOfRangeException(nameof(c));
            }

            this.Value = c;
        }

        public char Value { get; }

        protected override /*InquireResult*/ bool InquireImpl(IToken token, IResultAccumulator resultAccumulator)
        {
            var result =
                token is PunctuationToken punctuationToken &&
                punctuationToken.Value.Equals(this.Value);

            return result;

            //if (token is PunctuationToken punctuationToken && punctuationToken.Value.Equals(this.Value))
            //{
            //    return this.Action == null ? InquireResult.Skip : InquireResult.Act;
            //}
            //else
            //{
            //    return InquireResult.Reject;
            //}
        }
    }
}
