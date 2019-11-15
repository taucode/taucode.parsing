using System.Collections.Generic;

namespace TauCode.Parsing.Aide.Results
{
    public class BlockDefinitionResult2 : IAideResult
    {
        public BlockDefinitionResult2(string name)
        {
            this.Name = name;
            this.Content = new Content(this);
            this.Arguments = new List<string>();
        }

        public IContent Content { get; }
        public string Name { get; }
        public IList<string> Arguments { get; }
    }
}
