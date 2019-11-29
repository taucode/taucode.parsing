using TauCode.Parsing.Lexer2;
using TauCode.Parsing.TinyLisp.TokenExtractors;

namespace TauCode.Parsing.TinyLisp
{
    public class TinyLispLexer : LexerBase
    {
        public TinyLispLexer()
            : base(
                TinyLispHelper.IsSpace,
                TinyLispHelper.IsLineBreak)
        {
        }

        public bool AddCommentTokens { get; set; }

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

            // *** Links ***
            punctuationExtractor.AddSuccessors(
                commentExtractor,
                punctuationExtractor,
                keywordExtractor,
                symbolExtractor,
                stringExtractor);

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
                symbolExtractor);
        }
    }
}
