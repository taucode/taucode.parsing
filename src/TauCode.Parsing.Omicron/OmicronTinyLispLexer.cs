using TauCode.Parsing.Lexing;
using TauCode.Parsing.Omicron.Producers;
using TauCode.Parsing.TinyLisp;

namespace TauCode.Parsing.Omicron
{
    public class OmicronTinyLispLexer : OmicronLexerBase
    {
        protected override IOmicronTokenProducer[] CreateProducers()
        {
            return new IOmicronTokenProducer[]
            {
                new WhiteSpaceProducer(),
                new PunctuationProducer(),
                new StringProducer(),
                new IntegerProducer(IntegerTerminatorPredicate),
                new SymbolProducer(),
                new KeywordProducer(),
                new CommentProducer(),
            };
        }

        private static bool IntegerTerminatorPredicate(char c)
        {
            if (LexingHelper.IsInlineWhiteSpaceOrCaretControl(c))
            {
                return true;
            }

            if (TinyLispHelper.IsPunctuation(c))
            {
                return true;
            }

            return false;
        }
    }
}
