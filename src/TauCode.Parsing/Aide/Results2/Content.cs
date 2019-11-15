using System;
using System.Collections;
using System.Collections.Generic;

namespace TauCode.Parsing.Aide.Results2
{
    // todo: regions & nice
    public class Content : IContent
    {
        private readonly List<IAideResult2> _results;
        private bool _isSealed;

        public Content(IAideResult2 owner)
        {
            // todo checks
            this.Owner = owner;
            _results = new List<IAideResult2>();
        }

        public IEnumerator<IAideResult2> GetEnumerator() => _results.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _results.GetEnumerator();

        public int Count => _results.Count;

        public void Seal()
        {
            this.CheckNotSealed();
            _isSealed = true;
        }

        public bool IsSealed => _isSealed;

        public IAideResult2 this[int index] => _results[index];
        public IAideResult2 Owner { get; }

        public void AddResult(IAideResult2 result)
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
