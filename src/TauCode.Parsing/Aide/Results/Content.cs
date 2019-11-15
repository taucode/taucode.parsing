using System;
using System.Collections;
using System.Collections.Generic;

namespace TauCode.Parsing.Aide.Results
{
    // todo: regions & nice
    public class Content : IContent
    {
        private readonly List<IAideResult> _results;
        private bool _isSealed;

        public Content(IAideResult owner)
        {
            // todo checks
            this.Owner = owner;
            _results = new List<IAideResult>();
        }

        public IEnumerator<IAideResult> GetEnumerator() => _results.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _results.GetEnumerator();

        public int Count => _results.Count;

        public void Seal()
        {
            this.CheckNotSealed();
            _isSealed = true;
        }

        public bool IsSealed => _isSealed;

        public IAideResult this[int index] => _results[index];
        public IAideResult Owner { get; }

        public void AddResult(IAideResult result)
        {
            // todo checks
            this.CheckNotSealed();

            _results.Add(result);
        }

        private void CheckNotSealed()
        {
            if (_isSealed)
            {
                throw new NotImplementedException();
            }
        }
    }
}
