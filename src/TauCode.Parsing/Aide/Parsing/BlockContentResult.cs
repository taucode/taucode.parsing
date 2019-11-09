using System.Collections.Generic;
using System.Linq;

namespace TauCode.Parsing.Aide.Parsing
{
    public class BlockContentResult : UnitResult
    {
        private readonly List<UnitResult> _unitResults;

        public BlockContentResult()
            : base("todo")
        {
            this.Arguments = new NameReferenceCollector();
            _unitResults = new List<UnitResult>();
        }

        public NameReferenceCollector Arguments { get; } // actually, must contain exactly one name.

        public void AddUnitResult(UnitResult result)
        {
            _unitResults.Add(result);
        }

        public T GetLastUnitResult<T>() where T : UnitResult
        {
            return (T)_unitResults.Last(); // todo: optimize
        }
    }
}
