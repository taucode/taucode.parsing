using System.Collections.Generic;

namespace TauCode.Parsing.Aide.Parsing
{
    public class BlockResult
    {
        private readonly List<UnitResult> _unitResults;

        public BlockResult()
        {
            this.Name = new NameReferenceCollector();
            _unitResults = new List<UnitResult>();
        }

        public NameReferenceCollector Name { get; }

        public void AddUnitResult(UnitResult result)
        {
            _unitResults.Add(result);
        }
    }
}
