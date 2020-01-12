using TauCode.Extensions;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Old.Lexing;
using TauCode.Parsing.Old.TextClasses;
using TauCode.Parsing.Old.TextDecorations;
using TauCode.Parsing.Old.Tokens;

namespace TauCode.Parsing.Old.Tests.Parsing.Cli.TokenExtractors
{
    public class OldStringExtractor : OldTokenExtractorBase
    {
        private char? _openingDelimiter;

        public OldStringExtractor()
            : base(c => c.IsIn('\'', '"'))
        {
        }

        protected override void ResetState()
        {
            _openingDelimiter = null;
        }

        protected override IToken ProduceResult()
        {
            var str = this.ExtractResultString();
            var value = str.Substring(1, str.Length - 2);

            var position = new Position(this.StartLine, this.StartColumn);
            var consumedLength = this.LocalCharIndex;

            return new OldTextToken(
                OldStringTextClass.Instance,
                _openingDelimiter.Value == '"'
                    ? (IOldTextDecoration)OldDoubleQuoteTextDecoration.Instance
                    : (IOldTextDecoration)OldSingleQuoteTextDecoration.Instance,
                value,
                position,
                consumedLength);
        }

        protected override OldCharChallengeResult ChallengeCurrentChar()
        {
            var c = this.GetCurrentChar();


            var index = this.LocalCharIndex;

            if (index == 0)
            {
                _openingDelimiter = c;
                return OldCharChallengeResult.Continue; // 0th char MUST have been accepted.
            }

            if (LexingHelper.IsCaretControl(c))
            {
                throw new LexingException("Newline in string.", this.GetCurrentAbsolutePosition());
            }

            if (c == '\'' || c == '"')
            {
                if (c == _openingDelimiter.Value)
                {
                    this.Advance();
                    return OldCharChallengeResult.Finish;
                }

                return OldCharChallengeResult.Continue;
            }

            return OldCharChallengeResult.Continue;
        }

        protected override OldCharChallengeResult ChallengeEnd()
        {
            throw new LexingException("Unclosed string.", this.GetCurrentAbsolutePosition());
        }
    }
}
