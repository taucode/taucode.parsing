using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Aide.Results;

namespace TauCode.Parsing.Aide.Building
{
    public class Boss
    {
        private readonly List<BlockDefinitionResult> _blockDefinitions;

        public Boss(IEnumerable<IAideResult> results)
        {
            // todo: checks.
            _blockDefinitions = results.Cast<BlockDefinitionResult>().ToList();
            this.Squad = new Squad();
        }

        public INode Generate()
        {
            throw new System.NotImplementedException();
        }

        public Squad Squad { get; }
    }
}
