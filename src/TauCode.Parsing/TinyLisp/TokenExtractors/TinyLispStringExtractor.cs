using System;
using TauCode.Parsing.Lexer2;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.TinyLisp.TokenExtractors
{
    public class TinyLispStringExtractor : TokenExtractorBase
    {
        public TinyLispStringExtractor()
            : base(
                TinyLispHelper.IsSpace,
                TinyLispHelper.IsLineBreak,
                x => x == '"') // todo: consider extracting delegate into TinyLispHelper.
        {
        }

        protected override void Reset()
        {
            // idle
        }

        protected override IToken ProduceResult()
        {
            var str = this.ExtractResultString();
            var value = str.Substring(1, str.Length - 2);
            return new StringToken(value);
        }

        protected override TestCharResult TestCurrentChar()
        {
            var c = this.GetCurrentChar();
            var pos = this.GetLocalPosition();

            if (pos == 0)
            {
                return this.ContinueIf(c == '"');
            }

            if (c == '"')
            {
                this.Advance();
                return TestCharResult.Finish;
            }

            return TestCharResult.Continue;
        }

        protected override bool TestEnd()
        {
            throw new NotImplementedException();
        }
    }
}
