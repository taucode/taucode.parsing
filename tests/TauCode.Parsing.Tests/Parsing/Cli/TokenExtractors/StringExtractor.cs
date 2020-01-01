using System;
using TauCode.Extensions;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Tokens;
using TauCode.Parsing.Tokens.TextClasses;
using TauCode.Parsing.Tokens.TextDecorations;

namespace TauCode.Parsing.Tests.Parsing.Cli.TokenExtractors
{
    public class StringExtractor : TokenExtractorBase
    {
        public StringExtractor(ILexingEnvironment environment)
            : base(environment, c => c.IsIn('\'', '"'))
        {
        }

        protected override void ResetState()
        {
            // todo: _openingDelimiter = { '\'', '"' } ?..
        }

        protected override IToken ProduceResult()
        {
            var str = this.ExtractResultString();
            var value = str.Substring(1, str.Length - 2);
            return new TextToken(StringTextClass.Instance, NoneTextDecoration.Instance, value);
        }

        protected override CharChallengeResult ChallengeCurrentChar()
        {
            var c = this.GetCurrentChar();
            var pos = this.GetLocalPosition();

            if (pos == 0)
            {
                return CharChallengeResult.Continue; // 0th char MUST have been accepted.
            }

            if (this.Environment.IsLineBreak(c))
            {
                return CharChallengeResult.Error;
            }

            if (c == '\'' || c == '"')
            {
                var startingDelimiter = this.GetStartingDelimiter();
                if (c == startingDelimiter)
                {
                    this.Advance();
                    return CharChallengeResult.Finish;
                }

                return CharChallengeResult.Continue;
            }

            return CharChallengeResult.Continue;
        }

        private char GetStartingDelimiter()
        {
            return this.GetLocalChar(0);
        }

        protected override CharChallengeResult ChallengeEnd()
        {
            throw new NotImplementedException();
        }
    }
}
