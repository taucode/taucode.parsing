using System.Collections.Generic;

namespace TauCode.Parsing.ParsingUnits
{
    public interface IParsingNode : IParsingUnit
    {
        IReadOnlyList<IParsingUnit> Links { get; }
    }
}
