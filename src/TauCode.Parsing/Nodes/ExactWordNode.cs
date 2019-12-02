using System;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Nodes
{
    public class ExactWordNode : ActionNode
    {
        #region Constructor

        public ExactWordNode(
            string word,
            Action<IToken, IResultAccumulator> action,
            INodeFamily family,
            string name)
            : base(action, family, name)
        {
            this.Word = word ?? throw new ArgumentNullException(nameof(word));
        }


        #endregion

        #region Overridden

        protected override InquireResult InquireImpl(IToken token, IResultAccumulator resultAccumulator)
        {
            if (token is WordToken wordToken)
            {
                bool condition;
                if (this.IsCaseSensitive)
                {
                    condition = string.Equals(wordToken.Word, this.Word);
                }
                else
                {
                    condition = string.Equals(wordToken.Word, this.Word, StringComparison.InvariantCultureIgnoreCase);
                }

                if (condition)
                {
                    return this.Action == null ? InquireResult.Skip : InquireResult.Act;
                }
            }

            return InquireResult.Reject;
        }

        #endregion

        #region Public

        public string Word { get; }

        public bool IsCaseSensitive { get; set; }

        #endregion
    }
}
