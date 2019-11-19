using System.Collections.Generic;

namespace TauCode.Parsing.Aide.Results
{
    public class AlternativesResult : IAideResult
    {
        #region Fields

        private readonly List<Content> _alternatives;

        #endregion

        #region Constructor

        public AlternativesResult(string name)
        {
            this.Name = name;
            _alternatives = new List<Content>();
            this.AddAlternative();
            this.Arguments = new List<string>();
        }

        #endregion

        #region Public

        public void AddAlternative()
        {
            _alternatives.Add(new Content(this));
        }

        public Content GetLastAlternative()
        {
            return _alternatives[_alternatives.Count - 1]; // never empty.
        }

        public IList<Content> GetAllAlternatives() => _alternatives;

        #endregion

        #region IAideResult Members

        public string Name { get; }

        public List<string> Arguments { get; }

        #endregion
    }
}
