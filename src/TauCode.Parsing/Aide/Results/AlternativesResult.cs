using System;
using System.Collections.Generic;

namespace TauCode.Parsing.Aide.Results
{
    public class AlternativesResult : IAideResult2
    {
        private readonly List<Content> _alternatives;

        public AlternativesResult(string name)
        {
            this.Name = name;
            _alternatives = new List<Content>();
            this.AddAlternative();
        }

        public void AddAlternative()
        {
            _alternatives.Add(new Content(this));
        }

        public Content GetLastAlternative()
        {
            return _alternatives[_alternatives.Count - 1]; // never empty.
        }

        public IList<Content> GetAllAlternatives() => _alternatives;

        public string Name { get; }
        public IList<string> Arguments => throw new NotImplementedException();
    }
}
