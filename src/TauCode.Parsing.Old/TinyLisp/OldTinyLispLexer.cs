using TauCode.Parsing.Lexing;
using TauCode.Parsing.Lexing.StandardTokenExtractors;
using OldTinyLispCommentExtractor = TauCode.Parsing.Old.TinyLisp.TokenExtractors.OldTinyLispCommentExtractor;
using OldTinyLispKeywordExtractor = TauCode.Parsing.Old.TinyLisp.TokenExtractors.OldTinyLispKeywordExtractor;
using OldTinyLispPunctuationExtractor = TauCode.Parsing.Old.TinyLisp.TokenExtractors.OldTinyLispPunctuationExtractor;
using OldTinyLispStringExtractor = TauCode.Parsing.Old.TinyLisp.TokenExtractors.OldTinyLispStringExtractor;
using OldTinyLispSymbolExtractor = TauCode.Parsing.Old.TinyLisp.TokenExtractors.OldTinyLispSymbolExtractor;

namespace TauCode.Parsing.Old.TinyLisp
{
    public class OldTinyLispLexer : LexerBase
    {
        protected override void InitTokenExtractors()
        {
            // comment
            var commentExtractor = new OldTinyLispCommentExtractor();
            this.AddTokenExtractor(commentExtractor);

            // punctuation
            var punctuationExtractor = new OldTinyLispPunctuationExtractor();
            this.AddTokenExtractor(punctuationExtractor);

            // keyword
            var keywordExtractor = new OldTinyLispKeywordExtractor();
            this.AddTokenExtractor(keywordExtractor);

            // symbol
            var symbolExtractor = new OldTinyLispSymbolExtractor();
            this.AddTokenExtractor(symbolExtractor);

            // string
            var stringExtractor = new OldTinyLispStringExtractor();
            this.AddTokenExtractor(stringExtractor);

            // integer
            var integerExtractor = new IntegerExtractor();
            this.AddTokenExtractor(integerExtractor);

            // *** Links ***
            commentExtractor.AddSuccessors(
                commentExtractor,
                punctuationExtractor,
                keywordExtractor,
                symbolExtractor,
                stringExtractor,
                integerExtractor);

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
                commentExtractor,
                punctuationExtractor,
                stringExtractor);
        }
    }
}
