using TauCode.Extensions;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Lexing.StandardProducers;
using TauCode.Parsing.Tests.Parsing.Sql.Producers;

namespace TauCode.Parsing.Tests.Parsing.Sql
{
    public class SqlLexer : LexerBase
    {
        protected override ITokenProducer[] CreateProducers()
        {
            return new ITokenProducer[]
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
