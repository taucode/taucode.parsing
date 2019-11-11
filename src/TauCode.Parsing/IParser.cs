using System.Collections.Generic;

namespace TauCode.Parsing
{
    public interface IParser
    {
        IContext Parse(IEnumerable<IToken> tokens);
    }
}
