using System.Collections.Generic;

namespace TauCode.Parsing
{
    // todo: consider renaming to IParsingContext
    public interface IContext
    {
        ITokenStream TokenStream { get; }
        void SetNodes(IReadOnlyCollection<INode> nodes);
        IReadOnlyCollection<INode> GetNodes();
        IResultAccumulator ResultAccumulator { get; }
    }
}
