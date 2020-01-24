using System.Collections.Generic;

namespace TauCode.Parsing
{
    public interface IParsingContext
    {
        ITokenStream TokenStream { get; }
        void SetNodes(IReadOnlyCollection<INode> nodes);
        IReadOnlyCollection<INode> GetNodes();
        IResultAccumulator ResultAccumulator { get; }
    }
}
