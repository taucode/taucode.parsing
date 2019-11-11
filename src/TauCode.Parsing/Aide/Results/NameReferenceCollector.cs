using System.Collections.Generic;

namespace TauCode.Parsing.Aide.Results
{
    public class NameReferenceCollector
    {
        private readonly List<string> _names;

        public NameReferenceCollector()
        {
            _names = new List<string>();
        }

        public void Add(string name)
        {
            _names.Add(name);
        }
    }
}
