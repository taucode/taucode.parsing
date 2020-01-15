using TauCode.Parsing.Lexing;
using TauCode.Parsing.Omicron;
using TauCode.Parsing.Omicron.Producers;
using TauCode.Parsing.Tests.Parsing.Cli.Producers;

namespace TauCode.Parsing.Tests.Parsing.Cli
{
    public class CliLexer : OmicronLexerBase
    {
        protected override IOmicronTokenProducer[] CreateProducers()
        {
            return new IOmicronTokenProducer[]
            {
                new WhiteSpaceProducer(),
                new IntegerProducer(IsAcceptableIntegerTerminator), 
                new TermProducer(),
                new KeyProducer(),
                new CliSingleQuoteStringProducer(),
                new CliDoubleQuoteStringProducer(),
                new PathProducer(),
                new EqualsProducer(),
            };
        }

        private bool IsAcceptableIntegerTerminator(char c)
        {
            return LexingHelper.IsInlineWhiteSpaceOrCaretControl(c);
        }
    }
}
