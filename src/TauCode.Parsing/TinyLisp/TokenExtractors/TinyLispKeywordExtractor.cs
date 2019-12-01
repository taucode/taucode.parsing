using System;
using TauCode.Parsing.Lexizing;
using TauCode.Parsing.TinyLisp.Tokens;

namespace TauCode.Parsing.TinyLisp.TokenExtractors
{
    public class TinyLispKeywordExtractor : TokenExtractorBase
    {
        public TinyLispKeywordExtractor()
            : base(
                TinyLispHelper.IsSpace,
                TinyLispHelper.IsLineBreak,
                x => x == ':')
        {
        }

        protected override void Reset()
        {
            // idle
        }

        protected override IToken ProduceResult()
        {
            var res = this.ExtractResultString();

            return new KeywordToken(res);
        }

        protected override TestCharResult TestCurrentChar()
        {
            var c = this.GetCurrentChar();
            var pos = this.GetLocalPosition();

            if (pos == 0)
            {
                return this.ContinueIf(c == ':');
            }

            var isMine = c.IsAcceptableSymbolNameChar();
            if (isMine)
            {
                return TestCharResult.Continue;
            }

            return TestCharResult.Finish;
        }

        protected override bool TestEnd()
        {
            throw new NotImplementedException();
        }
    }
}
