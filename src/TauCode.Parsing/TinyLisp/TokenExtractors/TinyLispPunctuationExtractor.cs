using System.Linq;
using TauCode.Parsing.Exceptions;
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

            if (result.Length != 1)
            {
                // todo: copy-pasted exception.
                throw new LexerException("Internal error."); // how on earth we could even get here?
            }

            var c = result.Single();
            var punctuation = TinyLispHelper.CharToPunctuation(c);

            return new LispPunctuationToken(punctuation);
        }

        protected override CharChallengeResult TestCurrentChar()
        {
            var c = this.GetCurrentChar();
            var pos = this.GetLocalPosition();

            if (pos == 0)
            {
                if (TinyLispHelper.PunctuationChars.Contains(c))
                {
                    return CharChallengeResult.Continue;
                }
                else
                {
                    throw new LexerException("Internal error."); // how on earth we could even get here?
                }
            }

            return CharChallengeResult.Finish; // pos > 0 ==> finish no matter what.
        }

        protected override bool TestEnd() => true; // end after punctuation? why not, let it be.
    }
}
