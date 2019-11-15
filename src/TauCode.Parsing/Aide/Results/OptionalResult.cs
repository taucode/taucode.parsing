using System.Collections.Generic;

namespace TauCode.Parsing.Aide.Results
{
    public class OptionalResult : IAideResult
    {
        public OptionalResult(string name)
        {
            this.Name = name;
            this.OptionalContent = new Content(this);
            this.Arguments = new List<string>();
        }

        public Content OptionalContent { get; }
        public string Name { get; }
        public IList<string> Arguments { get; }
    }
}
