using System.Collections.Generic;
using TauCode.Parsing.Aide.Results;

namespace TauCode.Parsing.Aide.Building
{
    public class Builder : IBuilder
    {
        public INode Build(IEnumerable<IAideResult> results)
        {
            var buildWorker = new Boss(results);
            var node = buildWorker.Generate();
            return node;
        }
    }
}
