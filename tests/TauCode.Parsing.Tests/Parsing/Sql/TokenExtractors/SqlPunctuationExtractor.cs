using TauCode.Extensions;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.TextProcessing;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.Parsing.Sql.TokenExtractors
{
    // todo clean
    public class SqlPunctuationExtractor : TokenExtractorBase<PunctuationToken>
    {
        public SqlPunctuationExtractor()
            : base(new[]
            {
                typeof(PunctuationToken),
                typeof(TextToken),
                typeof(IntegerToken)
            })
        {

        }

        protected override void OnBeforeProcess()
        {
            this.AlphaCheckOnBeforeProcess();

            // idle
        }

        protected override PunctuationToken DeliverToken(string text, int absoluteIndex, Position position, int consumedLength)
        {
            return new PunctuationToken(text[absoluteIndex], position, consumedLength);
        }

        protected override CharAcceptanceResult AcceptCharImpl(char c, int localIndex)
        {
            if (localIndex == 0)
            {
                this.AlphaCheckNotBusyAndContextIsNull();
                return this.ContinueOrFail(c.IsIn('(', ')', ','));
            }

            return CharAcceptanceResult.Stop;
        }
    }
}
