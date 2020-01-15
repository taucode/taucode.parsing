using TauCode.Parsing.Lexing;
using TauCode.Parsing.Lexing.StandardProducers;
using TauCode.Parsing.Tests.Parsing.Cli.Producers;

namespace TauCode.Parsing.Tests.Parsing.Cli
{
    public class CliLexer : LexerBase
    {
        protected override ITokenProducer[] CreateProducers()
        {
            return new ITokenProducer[]
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
