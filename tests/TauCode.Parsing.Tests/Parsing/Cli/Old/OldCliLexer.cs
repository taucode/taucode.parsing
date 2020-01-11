using TauCode.Parsing.Old.Lexing;
using TauCode.Parsing.Old.Lexing.StandardTokenExtractors;
using TauCode.Parsing.Tests.Parsing.Cli.Old.TokenExtractors;

namespace TauCode.Parsing.Tests.Parsing.Cli.Old
{
    public class OldCliLexer : OldLexerBase
    {
        protected override void InitTokenExtractors()
        {
            // integer
            var integerExtractor = new OldIntegerExtractor();
            this.AddTokenExtractor(integerExtractor);

            // term
            var termExtractor = new OldTermExtractor();
            this.AddTokenExtractor(termExtractor);

            // key
            var keyExtractor = new OldKeyExtractor();
            this.AddTokenExtractor(keyExtractor);

            // string
            var stringExtractor = new OldStringExtractor();
            this.AddTokenExtractor(stringExtractor);

            // path
            var pathExtractor = new OldPathExtractor();
            this.AddTokenExtractor(pathExtractor);

            // equals
            var equalsExtractor = new OldEqualsExtractor();
            this.AddTokenExtractor(equalsExtractor);

            // *** Links ***
            keyExtractor.AddSuccessors(equalsExtractor);

            equalsExtractor.AddSuccessors(
                integerExtractor,
                termExtractor,
                keyExtractor,
                stringExtractor,
                pathExtractor);
        }
    }
}
