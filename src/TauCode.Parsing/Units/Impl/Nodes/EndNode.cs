using System;
using System.Collections.Generic;

namespace TauCode.Parsing.Units.Impl.Nodes
{
    public class EndNode : Node
    {
        public static EndNode Instance = new EndNode();

        private EndNode()
            : base(ParsingHelper.IdleTokenProcessor)
        {
            this.Name = "End";
        }

        protected override IReadOnlyList<IUnit> ProcessImpl(ITokenStream stream, IContext context)
        {
            throw new NotImplementedException(); // todo should never be called
        }

        protected override bool IsAcceptableToken(IToken token)
        {
            throw new NotImplementedException(); // todo should never be called
        }

        public override void AddLink(IUnit linked)
        {
            throw new NotImplementedException(); // todo: cannot add links to end node
        }

        protected override void OnBeforeFinalize()
        {
            // idle
        }
    }
}
