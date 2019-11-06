using System;
using System.Collections.Generic;

namespace TauCode.Parsing.ParsingUnits.Impl
{
    public class EndParsingNode : ParsingNode
    {
        public static EndParsingNode Instance = new EndParsingNode();

        private EndParsingNode()
            : base(ParsingHelper.IdleTokenProcessor)
        {
        }

        protected override IReadOnlyList<IParsingUnit> ProcessImpl(ITokenStream stream, IParsingContext context)
        {
            throw new NotImplementedException(); // todo should never be called
        }

        protected override bool IsAcceptableToken(IToken token)
        {
            throw new NotImplementedException(); // todo should never be called
        }
    }
}
