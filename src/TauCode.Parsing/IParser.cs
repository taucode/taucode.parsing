using System.Collections.Generic;

namespace TauCode.Parsing
{
    public interface IParser
    {
        IParsingContext Parse(IEnumerable<IToken> tokens);
    }
}
