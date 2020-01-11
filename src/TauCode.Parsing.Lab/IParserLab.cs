using System.Collections.Generic;

namespace TauCode.Parsing.Lab
{
    public interface IParserLab
    {
        bool WantsOnlyOneResult { get; set; }
        INode Root { get; set; }
        object[] Parse(IEnumerable<IToken> tokens);
    }
}
