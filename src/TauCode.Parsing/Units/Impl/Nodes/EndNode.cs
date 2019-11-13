using System;
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

        protected override IReadOnlyCollection<IUnit> ProcessImpl(ITokenStream stream, IContext context)
        {
            throw new ParserException($"The method '{typeof(EndNode).FullName}.{nameof(ProcessImpl)}' should never be called.");
        }

        public override void AddLink(IUnit linked)
        {
            throw new ParserException($"The method '{typeof(EndNode).FullName}.{nameof(AddLink)}' should never be called.");
        }

        public override void AddLinkAddress(string linkAddress)
        {
            throw new NotImplementedException();
        }

        protected override void OnBeforeFinalize()
        {
            // idle
        }

        public override IBlock Owner
        {
            get => null;
            internal set => throw new ParserException("Cannot set Owner of the EndNode.");
        }
    }
}
