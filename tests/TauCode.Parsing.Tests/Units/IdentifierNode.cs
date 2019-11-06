using System;
using TauCode.Parsing.ParsingUnits.Impl;
using TauCode.Parsing.Tests.Tokens;

namespace TauCode.Parsing.Tests.Units
{
    public class IdentifierNode : ParsingNode
    {
        public IdentifierNode(Action<IToken, IParsingContext> processor)
            : base(processor)
        {
        }

        //public override IReadOnlyList<IParsingUnit> Process(ITokenStream stream, IParsingContext context)
        //{
        //    var token = stream.GetCurrentToken();


        //    if (token is WordToken)
        //    {
        //        this.Processor(token, context);
        //        stream.AdvanceStreamPosition();
        //        return this.NextUnits;
        //    }

        //    return null;
        //}

        protected override bool IsAcceptableToken(IToken token)
        {
            return token is WordToken;
        }
    }
}
