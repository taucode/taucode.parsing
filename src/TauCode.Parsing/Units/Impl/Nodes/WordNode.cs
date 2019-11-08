using System;
using System.Diagnostics;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Units.Impl.Nodes
{
    [DebuggerDisplay("{" + nameof(Word) + "}")]
    public class WordNode : Node
    {
        public WordNode(string word, Action<IToken, IContext> processor)
            : base(processor)
        {
            this.Word = word ?? throw new ArgumentNullException(nameof(word));
        }

        public string Word { get; }

        // todo: remove comments
        //public override IReadOnlyList<IPa-rsingUnit> Process(ITokenStream stream, IPars-ingContext context)
        //{
        //    var token = stream.GetCurrentToken();

        //    if (
        //        token is WordToken wordToken &&
        //        wordToken.Word.Equals(this.Word, StringComparison.InvariantCultureIgnoreCase))
        //    {
        //        this.Processor(token, context);

        //        stream.AdvanceStreamPosition();
        //        return this.NextUnits;
        //    }

        //    return null;
        //}

        protected override bool IsAcceptableToken(IToken token)
        {
            return
                token is WordToken wordToken &&
                wordToken.Word.Equals(this.Word, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
