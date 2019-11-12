using System.Collections.Generic;
using TauCode.Parsing.Exceptions;

namespace TauCode.Parsing.Units.Impl.Nodes
{
    public sealed class EndNode : Node
    {
        public static EndNode Instance = new EndNode();

        private EndNode()
            : base("End")
        {
        }

        protected override IReadOnlyList<IUnit> ProcessImpl(ITokenStream stream, IContext context)
        {
            throw new ParserException($"The method '{typeof(EndNode).FullName}.{nameof(ProcessImpl)}' should never be called.");
        }

        public override void AddLink(IUnit linked)
        {
            throw new ParserException($"The method '{typeof(EndNode).FullName}.{nameof(AddLink)}' should never be called.");
        }

        protected override void OnBeforeFinalize()
        {
            // idle
        }
    }
}
