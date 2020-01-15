using TauCode.Parsing.Lexing;
using TauCode.Parsing.Lexing.StandardProducers;
using TauCode.Parsing.TinyLisp.Producers;

namespace TauCode.Parsing.TinyLisp
{
    public class TinyLispLexer : LexerBase
    {
        protected override ITokenProducer[] CreateProducers()
        {
            return new ITokenProducer[]
            {
                new WhiteSpaceProducer(),
                new TinyLispPunctuationProducer(),
                new TinyLispStringProducer(),
                new IntegerProducer(IntegerTerminatorPredicate),
                new TinyLispSymbolProducer(),
                new TinyLispKeywordProducer(),
                new TinyLispCommentProducer(),
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
