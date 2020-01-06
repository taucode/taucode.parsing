using System;
using TauCode.Parsing.Tokens;
using TauCode.Parsing.Tokens.TextClasses;
using TauCode.Parsing.Tokens.TextDecorations;

namespace TauCode.Parsing.Lexing.StandardTokenExtractors
{
    // todo clean up
    public class WordExtractor : TokenExtractorBase
    {
        public WordExtractor(
            //ILexingEnvironment environment,
            Func<char, bool> firstCharPredicate = null)
            : base(/*environment,*/ firstCharPredicate ?? StandardFirstCharPredicate)
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
            return new TextToken(
                WordTextClass.Instance,
                NoneTextDecoration.Instance,
                str,
                Position.TodoErrorPosition,
                LexingHelper.TodoErrorConsumedLength);
        }

        protected override CharChallengeResult ChallengeCurrentChar()
        {
            var c = this.GetCurrentChar();

            throw new NotImplementedException();
            //var pos = this.GetLocalPosition();

            //if (pos == 0)
            //{
            //    if (this.AllowsFirstChar(c))
            //    {
            //        return CharChallengeResult.Continue;
            //    }
            //    else
            //    {
            //        throw LexingHelper.CreateInternalErrorException();
            //    }
            //}

            
            //if (
            //    this.Environment.IsSpace(c) ||
            //    LexingHelper.IsStandardPunctuationChar(c))
            //{
            //    return CharChallengeResult.Finish;
            //}

            //if (this.AllowsInnerChar(c))
            //{
            //    return CharChallengeResult.Continue;
            //}

            //// I don't want this char inside my word.
            //return CharChallengeResult.GiveUp;
        }

        protected override CharChallengeResult ChallengeEnd() => CharChallengeResult.Finish; // word ended with end-of-input? no problem.
    }
}
