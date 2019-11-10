using System.Collections.Generic;

namespace TauCode.Parsing.Units.Impl.Nodes
{
    public class SplittingNode : Node
    {
        public SplittingNode(string name)
            : base(name)
        {

        }

        protected override IReadOnlyList<IUnit> ProcessImpl(ITokenStream stream, IContext context)
        {
            foreach (var link in this.Links)
            {
                IReadOnlyList<IUnit> result = link.Process(stream, context);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }
    }
}
