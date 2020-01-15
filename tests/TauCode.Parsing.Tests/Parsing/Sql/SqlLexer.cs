using TauCode.Extensions;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Omicron;
using TauCode.Parsing.Omicron.Producers;
using TauCode.Parsing.Tests.Parsing.Sql.Producers;

namespace TauCode.Parsing.Tests.Parsing.Sql
{
    public class SqlLexer : OmicronLexerBase
    {
        protected override IOmicronTokenProducer[] CreateProducers()
        {
            return new IOmicronTokenProducer[]
            {
                new WhiteSpaceProducer(),
                new WordProducer(),
                new SqlPunctuationProducer(),
                new IntegerProducer(IsAcceptableIntegerTerminator),
                new SqlIdentifierProducer(),
            };
        }

        private bool IsAcceptableIntegerTerminator(char c)
        {
            if (LexingHelper.IsInlineWhiteSpaceOrCaretControl(c))
            {
                return true;
            }

            if (c.IsIn('(', ')', ','))
            {
                return true;
            }

            return false;
        }
    }
}
