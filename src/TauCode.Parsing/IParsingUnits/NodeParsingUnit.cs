using System;
using System.Collections.Generic;

namespace TauCode.Parsing.IParsingUnits
{
    public class NodeParsingUnit : IParsingUnit
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
