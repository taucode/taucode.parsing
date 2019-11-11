using System;
using System.Collections.Generic;
using System.Linq;

namespace TauCode.Parsing.Aide.Results
{
    public class Content
    {
        private readonly List<UnitResult> _unitResults;

        public Content(IContentOwner owner)
        {
            this.Owner = owner ?? throw new ArgumentNullException(nameof(owner));
            _unitResults = new List<UnitResult>();
        }

        public IContentOwner Owner { get; }

        public void AddUnitResult(UnitResult unitResult)
        {
            if (unitResult == null)
            {
                throw new ArgumentNullException(nameof(unitResult));
            }

            _unitResults.Add(unitResult);
        }

        public UnitResult GetLastUnitResult()
        {
            return _unitResults.Last(); // todo: optimize, use concrete index.
        }

        public IList<UnitResult> GetAllResults() => _unitResults;

        public int UnitResultCount => _unitResults.Count;
    }
}
