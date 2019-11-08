using System.Collections.Generic;

namespace TauCode.Parsing.Aide.Parsing
{
    public class BlockDefinitionResult
    {
        private readonly List<UnitResult> _unitResults;

        public BlockDefinitionResult()
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
