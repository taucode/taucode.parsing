using System;
using System.Collections.Generic;

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
            if (_results.Count == 0)
            {
                throw new InvalidOperationException("Content is empty.");
            }

            return (T)_results[_results.Count - 1];
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
