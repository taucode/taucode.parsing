using System;
using TauCode.Extensions;
using TauCode.Parsing.Lexing;

namespace TauCode.Parsing.Tests.Parsing.Sql.TokenExtractors
{
    public class SqlIdentifierExtractor : TokenExtractorBase
    {
        public SqlIdentifierExtractor()
            : base(x => x.IsIn('[', '`', '"'))
        {
        }

        protected override void ResetState()
        {
            // idle now, but todo: _startingDelimiter = {'"', '[', '`'} ?..

        }

        protected override IToken ProduceResult()
        {
            var str = this.ExtractResultString();
            if (str.Length <= 2)
            {
                return null;
            }

            var identifier = str.Substring(1, str.Length - 2);
            throw new NotImplementedException();
            //return new TextToken(
            //    IdentifierTextClass.Instance,
            //    NoneTextDecoration.Instance,
            //    identifier);
        }

        protected override CharChallengeResult ChallengeCurrentChar()
        {
            var c = this.GetCurrentChar();

            throw new NotImplementedException();

            //var pos = this.GetLocalPosition();

            //if (pos == 0)
            //{
            //    return CharChallengeResult.Continue; // how else?
            //}

            //if (WordExtractor.StandardInnerCharPredicate(c))
            //{
            //    return CharChallengeResult.Continue;
            //}

            //if (c.IsIn(']', '`', '"'))
            //{
            //    var openingDelimiter = this.GetLocalChar(0);
            //    if (GetClosingDelimiter(openingDelimiter) == c)
            //    {
            //        this.Advance();
            //        return CharChallengeResult.Finish;
            //    }
            //}

            //return CharChallengeResult.Error; // unexpected char within identifier.
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

        protected override CharChallengeResult ChallengeEnd() => CharChallengeResult.Error; // met end while extracting identifier.
    }
}
