using TauCode.Parsing.Lexing;
using TauCode.Parsing.TextProcessing;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.Parsing.Cli.TokenExtractors
{
    public class EqualsExtractor : TokenExtractorBase<PunctuationToken>
    {
        public EqualsExtractor()
            : base(null)
        {
        }

        protected override void OnBeforeProcess()
        {
            this.AlphaCheckOnBeforeProcess();

            // idle
        }

        protected override PunctuationToken DeliverToken(string text, int absoluteIndex, Position position, int consumedLength)
        {
            return new PunctuationToken('=', position, consumedLength);
        }

        protected override CharAcceptanceResult AcceptCharImpl(char c, int localIndex)
        {
            if (localIndex == 0)
            {
                this.AlphaCheckNotBusyAndContextIsNull();
                return this.ContinueOrFail(c == '=');
            }

            return CharAcceptanceResult.Stop;
        }
    }
}
