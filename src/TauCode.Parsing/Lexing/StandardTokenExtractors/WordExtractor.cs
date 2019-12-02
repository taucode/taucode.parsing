using System;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Lexing.StandardTokenExtractors
{
    public class WordExtractor : TokenExtractorBase
    {
        public WordExtractor(
            ILexingEnvironment environment,
            Func<char, bool> firstCharPredicate = null)
            : base(environment, firstCharPredicate ?? StandardFirstCharPredicate)
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

        protected override IToken ProduceResult()
        {
            var str = this.ExtractResultString();
            return new WordToken(str);
        }

        protected override CharChallengeResult ChallengeCurrentChar()
        {
            var c = this.GetCurrentChar();
            var pos = this.GetLocalPosition();

            if (pos == 0)
            {
                if (this.AllowsFirstChar(c))
                {
                    return CharChallengeResult.Continue;
                }
                else
                {
                    throw new NotImplementedException(); // error, wtf.
                }
            }

            if (
                this.Environment.IsSpace(c) ||
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
