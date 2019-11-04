using System;
using System.Collections.Generic;

namespace TauCode.Parsing.ParsingUnits
{
    public class EndNodeParsingUnit : NodeParsingUnit
    {
        public static EndNodeParsingUnit Instance = new EndNodeParsingUnit();

        private EndNodeParsingUnit()
            : base(ParsingHelper.IdleTokenProcessor)
        {
        }

        public override IReadOnlyList<IParsingUnit> Process(ITokenStream stream, IParsingContext context)
        {
            throw new InvalidOperationException(); // todo
        }
    }
}
