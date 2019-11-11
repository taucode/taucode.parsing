using System;
using System.Collections.Generic;

namespace TauCode.Parsing.Aide.Results
{
    public class Content
    {
        private readonly List<UnitResult> _unitResults;
        private bool _isSealed;

        public Content(IContentOwner owner)
        {
            this.Owner = owner ?? throw new ArgumentNullException(nameof(owner));
            _unitResults = new List<UnitResult>();
        }

        public IContentOwner Owner { get; }

        public bool IsSealed => _isSealed;

        public void Seal()
        {
            if (_isSealed)
            {
                throw this.CreateContentIsSealedException();
            }

            _isSealed = true;
        }

        public void AddUnitResult(UnitResult unitResult)
        {
            if (_isSealed)
            {
                throw this.CreateContentIsSealedException();
            }

            if (unitResult == null)
            {
                throw new ArgumentNullException(nameof(unitResult));
            }

            _unitResults.Add(unitResult);
        }

        private InvalidOperationException CreateContentIsSealedException()
        {
            return new InvalidOperationException("Content is sealed");
        }

        public UnitResult GetLastUnitResult()
        {
            if (_unitResults.Count == 0)
            {
                throw new InvalidOperationException("Content is empty.");
            }

            return _unitResults[_unitResults.Count - 1];
        }

        public IList<UnitResult> GetAllResults() => _unitResults;

        public int UnitResultCount => _unitResults.Count;
    }
}
