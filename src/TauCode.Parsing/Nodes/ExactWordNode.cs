using System;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Nodes
{
    public class ExactWordNode : ActionNode
    {
        #region Constructor

        public ExactWordNode(INodeFamily family, string name, string word, Action<IToken, IResultAccumulator> action)
            : base(family, name, action)
        {
            this.Word = word ?? throw new ArgumentNullException(nameof(word));
        }


        #endregion

        #region Overridden

        protected override InquireResult InquireImpl(IToken token, IResultAccumulator resultAccumulator)
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

        #endregion

        #region Public

        public string Word { get; }

        #endregion
    }
}
