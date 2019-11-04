using System.Collections.Generic;

namespace TauCode.Parsing
{
    public interface IParsingUnit
    {
        IReadOnlyList<IParsingUnit> Process(ITokenStream stream, IParsingContext context);
        //IReadOnlyList<IParsingUnit> GetNextUnits(); // todo remove
    }
}
