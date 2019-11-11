using System.Collections;
using System.Collections.Generic;

namespace TauCode.Parsing.Aide.Results
{
    public class NameReferenceCollector : IEnumerable<string>
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

        public IEnumerator<string> GetEnumerator() => _names.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _names.GetEnumerator();
    }
}
