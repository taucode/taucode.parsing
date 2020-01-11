using System.Collections.Generic;

namespace TauCode.Parsing.Lab
{
    public class ParserLab : Parser, IParserLab
    {
        public INode Root { get; set; }
        public object[] Parse(IEnumerable<IToken> tokens) => this.ParseOld(this.Root, tokens);
    }
}
