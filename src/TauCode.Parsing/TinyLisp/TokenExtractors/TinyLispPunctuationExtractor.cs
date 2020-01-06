using System;
using System.Linq;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.TinyLisp.Tokens;

namespace TauCode.Parsing.TinyLisp.TokenExtractors
{
    // todo clean up
    public class TinyLispPunctuationExtractor : TokenExtractorBase
    {
        public TinyLispPunctuationExtractor()
            : base(
                //StandardLexingEnvironment.Instance,
                TinyLispHelper.IsPunctuation)
        {
        }

        protected override void ResetState()
        {
            // idle
        }

        protected override IToken ProduceResult()
        {
            var result = this.ExtractResultString();

            if (result.Length != 1)
            {
                throw LexingHelper.CreateInternalErrorException();
            }

            var c = result.Single();
            var punctuation = TinyLispHelper.CharToPunctuation(c);

            throw new NotImplementedException();

            //return new LispPunctuationToken(punctuation);
        }

        protected override CharChallengeResult ChallengeCurrentChar()
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
                    throw LexingHelper.CreateInternalErrorException();
                }
            }

            return CharChallengeResult.Finish; // pos > 0 ==> finish no matter what.
        }

        protected override CharChallengeResult ChallengeEnd() => CharChallengeResult.Finish; // end after punctuation? why not, let it be.
    }
}
