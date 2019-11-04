using System;
using System.Collections.Generic;

namespace TauCode.Parsing.ParsingUnits
{
    public class BlockParsingUnit : IParsingUnit
    {
        public BlockParsingUnit(NodeParsingUnit head)
        {
            this.Head = head ?? throw new ArgumentNullException(nameof(head));
        }

        public NodeParsingUnit Head { get; }


        public ParseResult Process(ITokenStream stream, IParsingContext context)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<IParsingUnit> GetNextUnits()
        {
            throw new NotImplementedException();
        }
    }
}
