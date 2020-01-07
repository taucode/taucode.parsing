using System.Collections.Generic;

namespace TauCode.Parsing
{
    public interface IParser
    {
        bool WantsOnlyOneResult { get; set; }
        object[] Parse(INode root, IEnumerable<IToken> tokens);
    }
}
