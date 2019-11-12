using System;
using TauCode.Parsing.Aide.Results;
using TauCode.Parsing.Units;

namespace TauCode.Parsing.Aide.Building
{
    public class AideBuildWorker
    {
        private readonly IAideResult[] _results;
        private readonly IBuildEnvironment _buildEnvironment;

        public AideBuildWorker(IAideResult[] results, IBuildEnvironment buildEnvironment)
        {
            _results = results;
            _buildEnvironment = buildEnvironment;
        }

        public IBlock BuildMainBlock()
        {
            throw new NotImplementedException();
        }
    }
}
