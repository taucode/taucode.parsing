using TauCode.Parsing.Aide.Results;
using TauCode.Parsing.Units;

namespace TauCode.Parsing.Aide.Building
{
    public class Builder : IBuilder
    {
        public IBlock BuildMainBlock(IAideResult[] structure, IBuildEnvironment buildEnvironment)
        {
            var worker = new BuildWorker(structure, buildEnvironment);
            var block = worker.BuildMainBlock();
            return block;
        }
    }
}
