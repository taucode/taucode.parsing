using System;
using TauCode.Parsing.ParsingUnits;
using TauCode.Parsing.Tests.Tokens;

namespace TauCode.Parsing.Tests.Units
{
    public class WordNodeParsingUnit : NodeParsingUnit
    {
        public WordNodeParsingUnit(string word, Action<IToken, IParsingContext> processor)
            : base(processor)
        {
            this.Word = word ?? throw new ArgumentNullException(nameof(word));
        }

        public string Word { get; }

        public override ParseResult Process(ITokenStream stream, IParsingContext context)
        {
            var token = stream.GetCurrentToken();

            var parseResult = ParseResult.Fail;

            if (
                token is WordToken wordToken &&
                wordToken.Word.Equals(this.Word, StringComparison.InvariantCultureIgnoreCase))
            {
                parseResult = ParseResult.Success;
                stream.AdvanceStreamPosition();
            }

            return parseResult;
        }
    }
}
