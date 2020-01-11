using System;
using TauCode.Parsing.Tokens;
using TauCode.Parsing.Tokens.TextClasses;
using TauCode.Parsing.Tokens.TextDecorations;

namespace TauCode.Parsing.Lexing.StandardTokenExtractors
{
    public class OldWordExtractor : TokenExtractorBase
    {
        public OldWordExtractor(Func<char, bool> firstCharPredicate = null)
            : base(firstCharPredicate ?? StandardFirstCharPredicate)
        {
        }

        public static bool StandardFirstCharPredicate(char c)
        {
            return c == '_' || LexingHelper.IsLatinLetter(c);
        }

        public static bool StandardInnerCharPredicate(char c)
        {
            return
                StandardFirstCharPredicate(c) ||
                LexingHelper.IsDigit(c);
        }

        protected virtual bool AllowsInnerChar(char c)
        {
            return StandardInnerCharPredicate(c);
        }

        protected override void ResetState()
        {
            // idle
        }

        protected override IToken ProduceResult()
        {
            var str = this.ExtractResultString();

            var position = new Position(this.StartingLine, this.StartingColumn);
            var consumedLength = this.LocalCharIndex;

            return new TextToken(
                WordTextClass.Instance,
                NoneTextDecoration.Instance,
                str,
                position,
                consumedLength);
        }

        protected override CharChallengeResult ChallengeCurrentChar()
        {
            var c = this.GetCurrentChar();

            var index = this.LocalCharIndex;

            if (index == 0)
            {
                return CharChallengeResult.Continue; // MUST be accepted in accordance with design.
            }

            if (
                LexingHelper.IsInlineWhiteSpaceOrCaretControl(c) ||
                LexingHelper.IsStandardPunctuationChar(c))
            {
                return CharChallengeResult.Finish;
            }

            if (this.AllowsInnerChar(c))
            {
                return CharChallengeResult.Continue;
            }

            // I don't want this char inside my word.
            return CharChallengeResult.GiveUp;
        }

        protected override CharChallengeResult ChallengeEnd() => CharChallengeResult.Finish; // word ended with end-of-input? no problem.
    }
}
