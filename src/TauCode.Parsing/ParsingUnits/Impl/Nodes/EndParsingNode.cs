using System;
using System.Collections.Generic;

namespace TauCode.Parsing.ParsingUnits.Impl.Nodes
{
    public class EndParsingNode : ParsingNode
    {
        public static EndParsingNode Instance = new EndParsingNode();

        private EndParsingNode()
            : base(ParsingHelper.IdleTokenProcessor)
        {
            this.Name = "End";
        }

        protected override IReadOnlyList<IParsingUnit> ProcessImpl(ITokenStream stream, IParsingContext context)
        {
            throw new NotImplementedException(); // todo should never be called
        }

        protected override bool IsAcceptableToken(IToken token)
        {
            throw new NotImplementedException(); // todo should never be called
        }

        public override void AddLink(IParsingUnit linked)
        {
            throw new NotImplementedException(); // todo: cannot add links to end node
        }

        protected override void OnBeforeFinalize()
        {
            // idle
        }
    }
}
