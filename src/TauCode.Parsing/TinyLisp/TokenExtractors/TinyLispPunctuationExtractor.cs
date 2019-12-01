using System.Linq;
using TauCode.Parsing.Lexizing;
using TauCode.Parsing.TinyLisp.Tokens;

namespace TauCode.Parsing.TinyLisp.TokenExtractors
{
    public class TinyLispPunctuationExtractor : TokenExtractorBase
    {
        public TinyLispPunctuationExtractor()
            : base(
                TinyLispHelper.IsSpace,
                TinyLispHelper.IsLineBreak, 
                TinyLispHelper.IsPunctuation)
        {
        }

        protected override void Reset()
        {
            // idle
        }

        protected override IToken ProduceResult()
        {
            var result = this.ExtractResultString();
            // todo: result must be exactly 1 char
            var c = result.Single();
            var punctuation = TinyLispHelper.CharToPunctuation(c);

            return new PunctuationToken(punctuation);
        }

        protected override TestCharResult TestCurrentChar()
        {
            var c = this.GetCurrentChar();
            var pos = this.GetLocalPosition();

            if (pos == 0)
            {
                return this.ContinueIf(TinyLispHelper.PunctuationChars.Contains(c)); // todo optimize with hash table
            }

            return TestCharResult.Finish; // pos > 0 ==> finish no matter what.
        }

        protected override bool TestEnd()
        {
            throw new System.NotImplementedException();
        }
    }
}
