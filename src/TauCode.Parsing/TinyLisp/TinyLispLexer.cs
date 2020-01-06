using TauCode.Parsing.Lexing;
using TauCode.Parsing.Lexing.StandardTokenExtractors;
using TauCode.Parsing.TinyLisp.TokenExtractors;

namespace TauCode.Parsing.TinyLisp
{
    // todo clean up; ctor not needed
    public class TinyLispLexer : LexerBase
    {
        public TinyLispLexer(/*ILexingEnvironment environment = null*/)
            : base(/*environment*/)
        {
        }

        protected override void InitTokenExtractors()
        {
            // comment
            var commentExtractor = new TinyLispCommentExtractor();
            this.AddTokenExtractor(commentExtractor);

            // punctuation
            var punctuationExtractor = new TinyLispPunctuationExtractor();
            this.AddTokenExtractor(punctuationExtractor);

            // keyword
            var keywordExtractor = new TinyLispKeywordExtractor();
            this.AddTokenExtractor(keywordExtractor);

            // symbol
            var symbolExtractor = new TinyLispSymbolExtractor();
            this.AddTokenExtractor(symbolExtractor);

            // string
            var stringExtractor = new TinyLispStringExtractor();
            this.AddTokenExtractor(stringExtractor);

            // integer
            var integerExtractor = new IntegerExtractor();
            this.AddTokenExtractor(integerExtractor);

            // *** Links ***
            punctuationExtractor.AddSuccessors(
                commentExtractor,
                punctuationExtractor,
                keywordExtractor,
                symbolExtractor,
                stringExtractor,
                integerExtractor);

            keywordExtractor.AddSuccessors(
                commentExtractor,
                punctuationExtractor,
                stringExtractor);

            symbolExtractor.AddSuccessors(
                commentExtractor,
                punctuationExtractor,
                stringExtractor);

            stringExtractor.AddSuccessors(
                commentExtractor,
                punctuationExtractor,
                keywordExtractor,
                symbolExtractor,
                integerExtractor);

            integerExtractor.AddSuccessors(
                punctuationExtractor,
                stringExtractor);
        }
    }
}
