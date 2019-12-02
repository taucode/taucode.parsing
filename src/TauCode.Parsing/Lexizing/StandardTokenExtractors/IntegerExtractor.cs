using System;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Lexizing.StandardTokenExtractors
{
    public class IntegerExtractor : TokenExtractorBase
    {
        public IntegerExtractor(
            Func<char, bool> spacePredicate,
            Func<char, bool> lineBreakPredicate)
            : base(
                spacePredicate,
                lineBreakPredicate,
                LexerHelper.IsIntegerFirstChar)
        {
        }

        protected override void Reset()
        {
            // idle
        }

        protected override IToken ProduceResult()
        {
            var str = this.ExtractResultString();
            return new IntegerToken(str);
        }

        protected override TestCharResult TestCurrentChar()
        {
            var c = this.GetCurrentChar();
            var pos = this.GetLocalPosition();

            if (pos == 0)
            {
                return this.ContinueIf(this.FirstCharPredicate(c));
            }

            if (LexerHelper.IsDigit(c))
            {
                return TestCharResult.Continue; // digits are always welcome in an integer.
            }

            if (c == '.' || c == '_')
            {
                // period ('.') is rarely a thing that you can delimit an integer within any grammar.
                // underscore ('_') is a punctuation mark, but nowadays it is usually a part of identifiers.

                return TestCharResult.NotAllowed;
            }

            // other punctuation marks like Comma, (, ), others might delimit though...
            if (char.IsPunctuation(c))
            {
                return TestCharResult.Finish;
            }

            if (this.SpacePredicate(c) || char.IsWhiteSpace(c))
            {
                return TestCharResult.Finish;
            }

            // other chars like letters and stuff => not allowed.
            return TestCharResult.NotAllowed;
        }

        protected override bool TestEnd()
        {
            var localPos = this.GetLocalPosition();

            if (localPos > 1)
            {
                // we consumed more than one char, so it is guaranteed we've got a good int already
                return true;
            }

            throw new NotImplementedException();
        }
    }
}
