using System;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Tests.Parsing.Cli.TextClasses;
using TauCode.Parsing.Tokens;
using TauCode.Parsing.Tokens.TextDecorations;

namespace TauCode.Parsing.Tests.Parsing.Cli.TokenExtractors
{
    public class KeyExtractor : TokenExtractorBase
    {
        public KeyExtractor()
            : base(c => c == '-')
        {
        }

        protected override void ResetState()
        {
        }

        protected override IToken ProduceResult()
        {
            var str = this.ExtractResultString();
            var position = new Position(this.StartingLine, this.StartingColumn);
            var consumedLength = this.LocalCharIndex;
            var token = new TextToken(KeyTextClass.Instance, NoneTextDecoration.Instance, str, position, consumedLength);
            return token;
        }

        protected override CharChallengeResult ChallengeCurrentChar()
        {
            var c = this.GetCurrentChar();
            var pos = this.LocalCharIndex;

            if (pos == 0)
            {
                return CharChallengeResult.Continue; // 0th char MUST have been accepted.
            }

            if (pos == 1)
            {
                if (c == '-')
                {
                    return CharChallengeResult.Continue;
                }

                if (LexingHelper.IsDigit(c) || LexingHelper.IsLatinLetter(c))
                {
                    return CharChallengeResult.Continue;
                }

                return CharChallengeResult.GiveUp;
            }

            if (pos == 2 && c == '-')
            {
                return CharChallengeResult.GiveUp; // 3 hyphens cannot be.
            }

            if (LexingHelper.IsDigit(c) || LexingHelper.IsLatinLetter(c))
            {
                return CharChallengeResult.Continue;
            }

            // todo: test keys "-", "--", "---", "--fo-", "-fo-", "---foo" etc.

            if (LexingHelper.IsInlineWhiteSpaceOrCaretControl(c) || c == '=')
            
            {
                return CharChallengeResult.Finish;
            }

            return CharChallengeResult.GiveUp;
        }

        protected override CharChallengeResult ChallengeEnd()
        {
            throw new NotImplementedException();
        }
    }
}
