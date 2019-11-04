using System;
using System.Collections.Generic;
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

        public override IReadOnlyList<IParsingUnit> Process(ITokenStream stream, IParsingContext context)
        {
            var token = stream.GetCurrentToken();

            
            if (token is WordToken)
            {
                this.Processor(token, context);
                stream.AdvanceStreamPosition();
                return this.NextUnits;
            }

            return null;
        }
    }
}
 