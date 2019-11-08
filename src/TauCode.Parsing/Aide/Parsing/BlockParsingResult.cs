using System.Collections.Generic;

namespace TauCode.Parsing.Aide.Parsing
{
    public class BlockParsingResult
    {
        private readonly List<ParsingUnitResult> _unitResults;

        public BlockParsingResult()
        {
            this.Name = new NameReferenceCollector();
            _unitResults = new List<ParsingUnitResult>();
        }

        public NameReferenceCollector Name { get; }

        public void AddUnitResult(ParsingUnitResult result)
        {
            _unitResults.Add(result);
        }
    }
}
