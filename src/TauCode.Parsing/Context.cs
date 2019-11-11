using System.Collections.Generic;
using System.Linq;

namespace TauCode.Parsing
{
    public class Context : IContext
    {
        private readonly List<object> _results;
        private int _version;

        public Context()
        {
            _results = new List<object>();
            _version = 1;
        }

        public void AddResult(object result)
        {
            _results.Add(result);
            this.Modify();
        }

        public T GetLastResult<T>()
        {
            return (T)_results.Last(); // todo: optimize, use index
        }

        public int ResultCount => _results.Count;

        public object[] ToArray()
        {
            return _results.ToArray();
        }

        public int Version => _version;

        public void Modify()
        {
            _version++;
        }
    }
}
