using System.Linq;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Old.Lexing;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Tokens;

namespace TauCode.Parsing.Old.TinyLisp.TokenExtractors
{
    public class OldTinyLispPunctuationExtractor : OldTokenExtractorBase
    {
        public OldTinyLispPunctuationExtractor()
            : base(TinyLispHelper.IsPunctuation)
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
                throw LexingHelper.CreateInternalErrorLexingException(this.GetCurrentAbsolutePosition());
            }

            var c = result.Single();
            var punctuation = TinyLispHelper.CharToPunctuation(c);

            var position = new Position(
                this.StartingLine,
                this.StartColumn);

            return new LispPunctuationToken(punctuation, position, this.LocalCharIndex);
        }

        protected override OldCharChallengeResult ChallengeCurrentChar()
        {
            if (this.LocalCharIndex == 0)
            {
                return OldCharChallengeResult.Continue; // local char MUST accepted since it was accepted by 'TinyLispHelper.IsPunctuation'
            }

            return OldCharChallengeResult.Finish; // pos > 0 ==> finish no matter what.
        }

        protected override OldCharChallengeResult ChallengeEnd() => OldCharChallengeResult.Finish; // end after punctuation? why not, let it be.
    }
}
