using System.Collections.Generic;

namespace TauCode.Parsing
{
    public interface IParser
    {
        bool WantsOnlyOneResult { get; set; }
        object[] ParseOld(INode root, IEnumerable<IToken> tokens);
    }
}
