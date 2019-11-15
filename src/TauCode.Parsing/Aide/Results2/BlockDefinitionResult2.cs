using System.Collections.Generic;

namespace TauCode.Parsing.Aide.Results2
{
    public class BlockDefinitionResult2 : IAideResult2
    {
        public BlockDefinitionResult2()
        {
            this.Content = new Content();
            this.Arguments = new List<string>();
        }

        public IContent Content { get; }
        public IList<string> Arguments { get; }
    }
}
