using System.Collections.Generic;

namespace TauCode.Parsing.Old
{
    public class OldParser : Parser, IOldParser
    {
        public object[] ParseOld(INode root, IEnumerable<IToken> tokens)
        {
            this.Root = root;
            return this.Parse(tokens);
        }
    }
}
