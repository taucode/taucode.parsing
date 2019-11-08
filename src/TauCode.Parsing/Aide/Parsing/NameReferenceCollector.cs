using System.Collections.Generic;

namespace TauCode.Parsing.Aide.Parsing
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
