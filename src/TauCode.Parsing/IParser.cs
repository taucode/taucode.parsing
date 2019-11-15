using System.Collections.Generic;

namespace TauCode.Parsing
{
    public interface IParser
    {
        object[] Parse(INode root, IEnumerable<IToken> tokens);
    }
}
