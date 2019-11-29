using TauCode.Parsing.Lexer2;
using TauCode.Parsing.TinyLisp.TokenExtractors;

namespace TauCode.Parsing.TinyLisp
{
    public class TinyLispLexer : LexerBase
    {
        public TinyLispLexer()
            : base(
                TinyLispHelper.SpaceChars,
                TinyLispHelper.LineBreakChars)
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

            // *** Links ***
            punctuationExtractor.AddSuccessors(
                commentExtractor,
                punctuationExtractor,
                keywordExtractor);

            keywordExtractor.AddSuccessors(
                commentExtractor,
                punctuationExtractor);

            //punctuationExtractor.AddSuccessors(
            //    commentExtractor,
            //    punctuationExtractor);

        }
    }
}
