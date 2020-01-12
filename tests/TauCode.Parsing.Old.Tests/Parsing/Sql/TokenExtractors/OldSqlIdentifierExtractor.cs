using TauCode.Extensions;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Old.Lexing;
using TauCode.Parsing.Old.Lexing.StandardTokenExtractors;
using TauCode.Parsing.Old.TextClasses;
using TauCode.Parsing.Old.TextDecorations;
using TauCode.Parsing.Old.Tokens;

namespace TauCode.Parsing.Old.Tests.Parsing.Sql.TokenExtractors
{
    public class OldSqlIdentifierExtractor : OldTokenExtractorBase
    {
        public OldSqlIdentifierExtractor()
            : base(x => x.IsIn('[', '`', '"'))
        {
        }

        protected override void ResetState()
        {
        }

        protected override IToken ProduceResult()
        {
            var str = this.ExtractResultString();
            if (str.Length <= 2)
            {
                return null;
            }

            var identifier = str.Substring(1, str.Length - 2);

            var position = new Position(this.StartLine, this.StartColumn);
            var consumedLength = this.LocalCharIndex;

            return new OldTextToken(
                OldIdentifierTextClass.Instance,
                OldNoneTextDecoration.Instance,
                identifier,
                position,
                consumedLength);
        }

        protected override OldCharChallengeResult ChallengeCurrentChar()
        {
            var c = this.GetCurrentChar();

            var index = this.LocalCharIndex;

            if (index == 0)
            {
                return OldCharChallengeResult.Continue; // how else?
            }

            if (OldWordExtractor.StandardInnerCharPredicate(c))
            {
                return OldCharChallengeResult.Continue;
            }

            if (c.IsIn(']', '`', '"'))
            {
                var openingDelimiter = this.GetLocalChar(0);
                if (GetClosingDelimiter(openingDelimiter) == c)
                {
                    this.Advance();
                    return OldCharChallengeResult.Finish;
                }
            }

            throw new LexingException("Unclosed identifier.", this.GetCurrentAbsolutePosition());
        }

        private char GetClosingDelimiter(char openingDelimiter)
        {
            switch (openingDelimiter)
            {
                case '[':
                    return ']';

                case '`':
                    return '`';

                case '"':
                    return '"';

                default:
                    return '\0';
            }
        }

        protected override OldCharChallengeResult ChallengeEnd()
        {
            throw new LexingException("Unclosed identifier.", this.GetCurrentAbsolutePosition());
        }
    }
}
