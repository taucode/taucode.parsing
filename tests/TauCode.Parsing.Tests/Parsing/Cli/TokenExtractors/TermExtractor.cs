﻿using TauCode.Parsing.Lexing;
using TauCode.Parsing.Tests.Parsing.Cli.TextClasses;
using TauCode.Parsing.Tokens;
using TauCode.Parsing.Tokens.TextDecorations;

namespace TauCode.Parsing.Tests.Parsing.Cli.TokenExtractors
{
    public class TermExtractor : TokenExtractorBase
    {
        public TermExtractor(ILexingEnvironment environment)
            : base(environment, IsTermFirstChar)
        {
        }

        private static bool IsTermFirstChar(char c) =>
            LexingHelper.IsDigit(c) ||
            LexingHelper.IsLatinLetter(c);

        protected override void ResetState()
        {
            // idle
        }


        protected override IToken ProduceResult()
        {
            var str = this.ExtractResultString();
            //return new TermToken(str);
            var token = new TextToken(TermTextClass.Instance, NoneTextDecoration.Instance, str);
            return token;
        }

        protected override CharChallengeResult ChallengeCurrentChar()
        {
            var c = this.GetCurrentChar();
            var pos = this.GetLocalPosition();

            if (pos == 0)
            {
                return CharChallengeResult.Continue; // 0th char MUST have been accepted.
            }

            if (c == '-')
            {
                if (this.GetPreviousChar() == '-') // todo: move to parsing lib of taucode
                {
                    return CharChallengeResult.GiveUp;
                }

                return CharChallengeResult.Continue;
            }

            if (LexingHelper.IsDigit(c) || LexingHelper.IsLatinLetter(c))
            {
                return CharChallengeResult.Continue;
            }

            if (this.Environment.IsSpace(c))
            {
                if (this.GetPreviousChar() == '-')
                {
                    return CharChallengeResult.GiveUp; // term cannot end with '-'
                }
                else
                {
                    return CharChallengeResult.Finish;
                }
            }

            return CharChallengeResult.GiveUp;
        }

        private char GetPreviousChar()
        {
            return this.GetLocalChar(this.GetLocalPosition() - 1);
        }

        protected override CharChallengeResult ChallengeEnd()
        {
            if (this.GetPreviousChar() == '-')
            {
                return CharChallengeResult.GiveUp;
            }

            return CharChallengeResult.Finish;
        }
    }
}
