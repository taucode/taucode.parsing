using System;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.TinyLisp.Tokens;

namespace TauCode.Parsing.TinyLisp.TokenExtractors
{
    // todo clean up
    public class TinyLispKeywordExtractor : TokenExtractorBase
    {
        public TinyLispKeywordExtractor()
            : base(
                //StandardLexingEnvironment.Instance,
                x => x == ':')
        {
        }

        protected override void ResetState()
        {
            // idle
        }

        protected override IToken ProduceResult()
        {
            var res = this.ExtractResultString();

            var position = new Position(this.StartingLine, this.StartingColumn);
            var consumedLength = this.LocalCharIndex;

            return new KeywordToken(res, position, consumedLength);
        }

        protected override CharChallengeResult ChallengeCurrentChar()
        {
            var c = this.GetCurrentChar();
            var pos = this.LocalCharIndex;

            if (pos == 0)
            {
                return CharChallengeResult.Continue; // 0th char is always ok
            }

            var isMine = c.IsAcceptableSymbolNameChar();
            if (isMine)
            {
                return CharChallengeResult.Continue;
            }

            return CharChallengeResult.Finish;
        }

        protected override CharChallengeResult ChallengeEnd()
        {
            throw new NotImplementedException();
            //if (this.GetLocalPosition() > 1)
            //{
            //    // consumed more than one char (0th is always ':'), so no problem here
            //    return CharChallengeResult.Finish;
            //}
            //else
            //{
            //    // consumed just one char (':'), therefore error. on one other token extractor in LISP can have ':' at the beginning.
            //    return CharChallengeResult.Error;
            //}
        }
    }
}
