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
                new WhiteSpaceProducer(), // NB: it is very important that this one goes first. it will skip white spaces without producing a real token.
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
