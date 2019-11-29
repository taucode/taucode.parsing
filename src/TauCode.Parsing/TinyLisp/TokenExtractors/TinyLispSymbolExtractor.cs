using System;
using TauCode.Parsing.Lexer2;
using TauCode.Parsing.TinyLisp.Tokens;

namespace TauCode.Parsing.TinyLisp.TokenExtractors
{
    public class TinyLispSymbolExtractor : TokenExtractorBase
    {
        public TinyLispSymbolExtractor() 
            : base(
                TinyLispHelper.IsSpace,
                TinyLispHelper.IsLineBreak,
                TinyLispHelper.IsAcceptableSymbolNameChar)
        {
        }

        protected override void Reset()
        {
            // idle
        }

        protected override IToken ProduceResult()
        {
            var res = this.ExtractResultString();
            return new LispSymbolToken(res);
        }

        protected override TestCharResult TestCurrentChar()
        {
            var c = this.GetCurrentChar();
            //var pos = this.GetLocalPosition();

            if (c.IsAcceptableSymbolNameChar())
            {
                return TestCharResult.Continue; // todo: deal with integers, doubles and ratios.
            }

            return TestCharResult.Finish;
        }

        protected override bool TestEnd()
        {
            throw new NotImplementedException();
        }
    }
}
