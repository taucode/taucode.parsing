using System;
using System.Collections.Generic;

namespace TauCode.Parsing.IParsingUnits
{
    public class BlockParsingUnit : IParsingUnit
    {
        public ParseResult Process()
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<IParsingUnit> GetNextUnits()
        {
            throw new NotImplementedException();
        }
    }
}
