using System.Collections.Generic;

namespace TauCode.Parsing
{
    public interface IContext
    {
        ITokenStream TokenStream { get; }
        void SetNodes(IReadOnlyCollection<INode> nodes);
        IReadOnlyCollection<INode> GetNodes();
        IResultAccumulator ResultAccumulator { get; }
    }
}
