using System.Collections.Generic;

namespace TauCode.Parsing
{
    public interface IParser2
    {
        object[] Parse(INode2 root, IEnumerable<IToken> tokens);
    }
}
