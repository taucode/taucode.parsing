using System;
using TauCode.Parsing.ParsingUnits;
using TauCode.Parsing.Tests.Tokens;

namespace TauCode.Parsing.Tests.Units
{
    public class IdentifierNodeParsingUnit : NodeParsingUnit
    {
        public IdentifierNodeParsingUnit(Action<IToken, IParsingContext> processor)
            : base(processor)
        {
        }

        public override ParseResult Process(ITokenStream stream, IParsingContext context)
        {
            var token = stream.GetCurrentToken();

            var parseResult = ParseResult.Fail;

            if (token is WordToken)
            {
                this.Processor(token, context);

                parseResult = ParseResult.Success;
                stream.AdvanceStreamPosition();
            }

            return parseResult;

        }
    }
}
 