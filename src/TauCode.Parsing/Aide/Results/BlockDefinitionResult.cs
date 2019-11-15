using System.Collections.Generic;

namespace TauCode.Parsing.Aide.Results
{
    public class BlockDefinitionResult : IAideResult
    {
        public BlockDefinitionResult(string name)
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
