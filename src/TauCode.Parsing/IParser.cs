using System.Collections.Generic;

namespace TauCode.Parsing
{
    public interface IParser
    {
        bool WantsOnlyOneResult { get; set; }
        INode Root { get; set; }
        object[] Parse(IEnumerable<IToken> tokens);
    }
}
