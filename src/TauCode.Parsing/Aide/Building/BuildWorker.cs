using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Aide.Results;

namespace TauCode.Parsing.Aide.Building
{
    public class BuildWorker
    {
        private readonly List<BlockDefinitionResult> _blockDefinitions;

        public BuildWorker(IEnumerable<IAideResult> results)
        {
            // todo: checks.
            _blockDefinitions = results.Cast<BlockDefinitionResult>().ToList();
        }

        public INode Generate()
        {
            throw new System.NotImplementedException();
        }
    }
}
