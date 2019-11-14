using System;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Nodes2
{
    public class ExactWordNode : ActionNode
    {
        public ExactWordNode(INodeFamily family, string name, string word, Action<IToken, IResultAccumulator> action)
            : base(family, name, action)
        {
            this.Word = word ?? throw new ArgumentNullException(nameof(word));
        }

        public string Word { get; }

        protected override InquireResult InquireImpl(IToken token)
        {
            if (token is WordToken wordToken && wordToken.Word == this.Word)
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
