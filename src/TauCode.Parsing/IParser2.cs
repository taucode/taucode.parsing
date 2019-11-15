using System.Collections.Generic;

namespace TauCode.Parsing
{
    public interface IParser2
    {
        object[] Parse(INode root, IEnumerable<IToken> tokens);
    }
}
