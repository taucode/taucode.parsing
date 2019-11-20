using System.Collections.Generic;
using TauCode.Parsing.Aide.Results;

namespace TauCode.Parsing.Aide.Building
{
    public interface IBuilder
    {
        INode Build(string nodeFamilyName, IEnumerable<IAideResult> results);
    }
}
