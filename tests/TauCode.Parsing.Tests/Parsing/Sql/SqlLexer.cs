using TauCode.Parsing.Lexing;
using TauCode.Parsing.Lexing.StandardTokenExtractors;
using TauCode.Parsing.Tests.Parsing.Sql.TokenExtractors;

namespace TauCode.Parsing.Tests.Parsing.Sql
{
    public class SqlLexer : LexerBase
    {
        public SqlLexer()
        {
        }

        protected override void InitTokenExtractors()
        {
            // word
            var wordExtractor = new WordExtractor(this.Environment);
            this.AddTokenExtractor(wordExtractor);

            // punctuation
            var punctuationExtractor = new SqlPunctuationExtractor();
            this.AddTokenExtractor(punctuationExtractor);

            // integer
            var integerExtractor = new IntegerExtractor(this.Environment);
            this.AddTokenExtractor(integerExtractor);

            // identifier
            var identifierExtractor = new SqlIdentifierExtractor();
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
