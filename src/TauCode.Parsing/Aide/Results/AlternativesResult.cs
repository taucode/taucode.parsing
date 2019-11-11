﻿using System.Collections.Generic;
using System.Linq;

namespace TauCode.Parsing.Aide.Results
{
    public class AlternativesResult : UnitResult, IContentOwner
    {
        private readonly List<Content> _alternatives;

        public AlternativesResult(string sourceNodeName)
            : base(sourceNodeName)
        {
            _alternatives = new List<Content>();
            this.AddAlternative();
        }

        public void AddAlternative()
        {
            _alternatives.Add(new Content(this));
        }

        public Content GetLastAlternative()
        {
            return _alternatives.Last(); // todo: exact index (optimize)
        }

        public IList<Content> GetAllAlternatives() => _alternatives;
    }
}
