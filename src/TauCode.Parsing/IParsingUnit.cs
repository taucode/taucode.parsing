using System.Collections.Generic;

namespace TauCode.Parsing
{
    public interface IParsingUnit
    {
        ParseResult Process(ITokenStream stream, IParsingContext context);
        IReadOnlyList<IParsingUnit> GetNextUnits();
    }
}
