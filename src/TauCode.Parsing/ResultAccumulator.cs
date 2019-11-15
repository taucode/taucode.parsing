using System.Collections;
using System.Collections.Generic;

namespace TauCode.Parsing
{
    public class ResultAccumulator : IResultAccumulator
    {
        #region Fields

        private readonly List<object> _results;

        #endregion

        #region Constructor

        public ResultAccumulator()
        {
            _results = new List<object>();
            this.Version = 1;
        }

        #endregion

        #region IReadOnlyList<object> Members

        public IEnumerator<object> GetEnumerator() => _results.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _results.GetEnumerator();

        public int Count => _results.Count;

        public object this[int index] => _results[index];

        #endregion

        #region IResultAccumulator Members

        public void Modify()
        {
            this.Version++;
        }

        public int Version { get; private set; }

        public void AddResult(object result)
        {
            // todo: checks
            _results.Add(result);
        }

        #endregion
    }
}
