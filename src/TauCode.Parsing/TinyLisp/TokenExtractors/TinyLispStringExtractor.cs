using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Tokens;
using TauCode.Parsing.Tokens.TextClasses;
using TauCode.Parsing.Tokens.TextDecorations;

namespace TauCode.Parsing.TinyLisp.TokenExtractors
{
    // todo: clean up
    public class TinyLispStringExtractor : TokenExtractorBase
    {
        public TinyLispStringExtractor()
            : base(
                //StandardLexingEnvironment.Instance,
                x => x == '"')
        {
        }

        protected override void ResetState()
        {
            // idle
        }

        protected override IToken ProduceResult()
        {
            var str = this.ExtractResultString();
            var value = str.Substring(1, str.Length - 2);

            var position = new Position(this.StartingLine, this.StartingColumn);
            var consumedLength = this.LocalCharIndex;

            return new TextToken(
                StringTextClass.Instance,
                DoubleQuoteTextDecoration.Instance,
                value,
                position,
                consumedLength);
        }

        protected override CharChallengeResult ChallengeCurrentChar()
        {
            var c = this.GetCurrentChar();
            var pos = this.LocalCharIndex;

            if (pos == 0)
            {
                return CharChallengeResult.Continue;
            }

            if (LexingHelper.IsCaretControl(c))
            {
                throw new LexingException("Newline in string.", this.GetCurrentAbsolutePosition());
            }

            if (c == '"')
            {
                this.Advance();
                return CharChallengeResult.Finish;
            }

            return CharChallengeResult.Continue;
        }

        protected override CharChallengeResult ChallengeEnd()
        {
            throw new LexingException("Unclosed string.", this.GetStartingAbsolutePosition());
        }
    }
}
