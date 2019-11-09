using System.Collections.Generic;

namespace TauCode.Parsing.Aide.Parsing
{
    public class LinkResult : UnitResult
    {
        private readonly List<string> _arguments;

        public LinkResult(string tag) 
            : base(tag)
        {
            _arguments = new List<string>();
        }

        public void AddArgument(string argument)
        {
            _arguments.Add(argument);
        }
    }
}
