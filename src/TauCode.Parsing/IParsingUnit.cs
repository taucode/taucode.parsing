using System.Collections.Generic;

namespace TauCode.Parsing
{
    public interface IParsingUnit
    {
        ParseResult Process();
        IReadOnlyList<IParsingUnit> GetNextUnits();
    }
}
