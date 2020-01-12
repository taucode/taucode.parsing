using System;
using TauCode.Extensions;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.Parsing.Sql.TokenExtractors
{
    public class SqlPunctuationExtractor : TokenExtractorBase<PunctuationToken>
    {
        public override PunctuationToken ProduceToken(string text, int absoluteIndex, int consumedLength, Position position)
        {
            return new PunctuationToken(text[absoluteIndex], position, consumedLength);
        }

        protected override void OnBeforeProcess()
        {
            // todo: temporary check that IsProcessing == FALSE, everywhere
            if (this.IsProcessing)
            {
                throw new NotImplementedException();
            }

            // todo: temporary check that LocalPosition == 1, everywhere
            if (this.Context.GetLocalIndex() != 1)
            {
                throw new NotImplementedException();
            }

            // idle
        }

        protected override bool AcceptsPreviousTokenImpl(IToken previousToken)
        {
            return
                previousToken is PunctuationToken ||
                previousToken is TextToken ||
                previousToken is IntegerToken;
        }

        protected override CharAcceptanceResult AcceptCharImpl(char c, int localIndex)
        {
            if (localIndex == 0)
            {
                return this.ContinueOrFail(c.IsIn('(', ')', ','));
            }

            return CharAcceptanceResult.Stop;
        }
    }
}
