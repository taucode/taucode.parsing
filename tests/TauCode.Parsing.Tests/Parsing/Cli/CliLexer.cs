using TauCode.Parsing.Lexing;
using TauCode.Parsing.Lexing.StandardTokenExtractors;
using TauCode.Parsing.Tests.Parsing.Cli.TokenExtractors;

namespace TauCode.Parsing.Tests.Parsing.Cli
{
    public class CliLexer : LexerBase
    {
        protected override void InitTokenExtractors()
        {
            // integer
            var integerExtractor = new IntegerExtractor(this.Environment);
            this.AddTokenExtractor(integerExtractor);

            // term
            var termExtractor = new TermExtractor(this.Environment);
            this.AddTokenExtractor(termExtractor);

            // key
            var keyExtractor = new KeyExtractor(this.Environment);
            this.AddTokenExtractor(keyExtractor);

            // string
            var stringExtractor = new StringExtractor(this.Environment);
            this.AddTokenExtractor(stringExtractor);

            // path
            var pathExtractor = new PathExtractor(this.Environment);
            this.AddTokenExtractor(pathExtractor);
        }
    }
}
