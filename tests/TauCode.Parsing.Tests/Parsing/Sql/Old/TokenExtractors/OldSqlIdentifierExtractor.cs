﻿using TauCode.Extensions;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Lexing.StandardTokenExtractors;
using TauCode.Parsing.Tokens;
using TauCode.Parsing.Tokens.TextClasses;
using TauCode.Parsing.Tokens.TextDecorations;

namespace TauCode.Parsing.Tests.Parsing.Sql.Old.TokenExtractors
{
    public class OldSqlIdentifierExtractor : TokenExtractorBase
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

            var position = new Position(this.StartingLine, this.StartingColumn);
            var consumedLength = this.LocalCharIndex;

            return new TextToken(
                IdentifierTextClass.Instance,
                NoneTextDecoration.Instance,
                identifier,
                position,
                consumedLength);
        }

        protected override CharChallengeResult ChallengeCurrentChar()
        {
            var c = this.GetCurrentChar();

            var index = this.LocalCharIndex;

            if (index == 0)
            {
                return CharChallengeResult.Continue; // how else?
            }

            if (WordExtractor.StandardInnerCharPredicate(c))
            {
                return CharChallengeResult.Continue;
            }

            if (c.IsIn(']', '`', '"'))
            {
                var openingDelimiter = this.GetLocalChar(0);
                if (GetClosingDelimiter(openingDelimiter) == c)
                {
                    this.Advance();
                    return CharChallengeResult.Finish;
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

        protected override CharChallengeResult ChallengeEnd()
        {
            throw new LexingException("Unclosed identifier.", this.GetCurrentAbsolutePosition());
        }
    }
}
