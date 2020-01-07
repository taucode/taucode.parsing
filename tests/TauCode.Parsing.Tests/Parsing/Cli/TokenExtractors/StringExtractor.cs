using TauCode.Extensions;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Tokens;
using TauCode.Parsing.Tokens.TextClasses;
using TauCode.Parsing.Tokens.TextDecorations;

namespace TauCode.Parsing.Tests.Parsing.Cli.TokenExtractors
{
    public class StringExtractor : TokenExtractorBase
    {
        private char? _startingDelimiter;

        public StringExtractor()
            : base(c => c.IsIn('\'', '"'))
        {
        }

        protected override void ResetState()
        {
            _startingDelimiter = null;
        }

        protected override IToken ProduceResult()
        {
            var str = this.ExtractResultString();
            var value = str.Substring(1, str.Length - 2);

            var position = new Position(this.StartingLine, this.StartingColumn);
            var consumedLength = this.LocalCharIndex;

            return new TextToken(
                StringTextClass.Instance,
                _startingDelimiter.Value == '"'
                    ? (ITextDecoration)DoubleQuoteTextDecoration.Instance
                    : (ITextDecoration)SingleQuoteTextDecoration.Instance,
                value,
                position,
                consumedLength);
        }

        protected override CharChallengeResult ChallengeCurrentChar()
        {
            var c = this.GetCurrentChar();


            var index = this.LocalCharIndex;

            if (index == 0)
            {
                _startingDelimiter = c;
                return CharChallengeResult.Continue; // 0th char MUST have been accepted.
            }

            if (LexingHelper.IsCaretControl(c))
            {
                throw new LexingException("Newline in string.", this.GetCurrentAbsolutePosition());
            }

            if (c == '\'' || c == '"')
            {
                if (c == _startingDelimiter.Value)
                {
                    this.Advance();
                    return CharChallengeResult.Finish;
                }

                return CharChallengeResult.Continue;
            }

            return CharChallengeResult.Continue;
        }

        protected override CharChallengeResult ChallengeEnd()
        {
            throw new LexingException("Unclosed string.", this.GetCurrentAbsolutePosition());
        }
    }
}
