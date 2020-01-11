using TauCode.Parsing.Lexing;
using TauCode.Parsing.Lexing.StandardTokenExtractors;
using TauCode.Parsing.Tests.Parsing.Sql.Old.TokenExtractors;

namespace TauCode.Parsing.Tests.Parsing.Sql.Old
{
    public class OldSqlLexer : OldLexerBase
    {
        protected override void InitTokenExtractors()
        {
            // word
            var wordExtractor = new OldWordExtractor();
            this.AddTokenExtractor(wordExtractor);

            // punctuation
            var punctuationExtractor = new OldSqlPunctuationExtractor();
            this.AddTokenExtractor(punctuationExtractor);

            // integer
            var integerExtractor = new OldIntegerExtractor();
            this.AddTokenExtractor(integerExtractor);

            // identifier
            var identifierExtractor = new OldSqlIdentifierExtractor();
            this.AddTokenExtractor(identifierExtractor);

            // *** Links ***
            wordExtractor.AddSuccessors(
                punctuationExtractor);

            punctuationExtractor.AddSuccessors(
                punctuationExtractor,
                wordExtractor,
                integerExtractor,
                identifierExtractor);

            integerExtractor.AddSuccessors(
                punctuationExtractor);

            identifierExtractor.AddSuccessors(
                punctuationExtractor);
        }
    }
}
