using System.Collections.Generic;

namespace TauCode.Parsing.Aide.Parsing
{
    public class BlockDefinitionResult
    {
        private readonly List<UnitResult> _unitResults;

        public BlockDefinitionResult()
        {
            this.Arguments = new NameReferenceCollector();
            _unitResults = new List<UnitResult>();
        }

        public NameReferenceCollector Arguments { get; } // actually, must contain exactly one name.

        public void AddUnitResult(UnitResult result)
        {
            _unitResults.Add(result);
        }
    }
}
