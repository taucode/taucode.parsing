using System.Collections.Generic;

namespace TauCode.Parsing.Old
{
    public interface IOldParser
    {
        bool WantsOnlyOneResult { get; set; }
        object[] ParseOld(INode root, IEnumerable<IToken> tokens);
    }
}
