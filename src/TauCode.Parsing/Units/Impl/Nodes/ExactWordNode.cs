using System;
using System.Diagnostics;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Units.Impl.Nodes
{
    [DebuggerDisplay("{" + nameof(Word) + "}")]
    public class ExactWordNode : ProcessingNode
    {
        public ExactWordNode(string word, Action<IToken, IContext> processor, string name)
            : base(processor, name)
        {
            this.Word = word ?? throw new ArgumentNullException(nameof(word));
        }

        public string Word { get; }

        protected override bool IsAcceptableToken(IToken token)
        {
            return
                token is WordToken wordToken &&
                wordToken.Word.Equals(this.Word, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
