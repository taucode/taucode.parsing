using System.Collections.Generic;
using TauCode.Parsing.Aide.Results;

namespace TauCode.Parsing.Aide.Building
{
    public class Builder : IBuilder
    {
        public INode Build(IEnumerable<IAideResult> results)
        {
            var boss = new Boss(results);
            var node = boss.Deliver();
            return node;
        }
    }
}
