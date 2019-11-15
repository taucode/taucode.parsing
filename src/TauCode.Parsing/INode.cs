using System.Collections.Generic;

namespace TauCode.Parsing
{
    public interface INode
    {
        INodeFamily Family { get; }
        string Name { get; }
        InquireResult Inquire(IToken token, IResultAccumulator resultAccumulator);
        void Act(IToken token, IResultAccumulator resultAccumulator);
        void AddLink(INode node);
        void AddLinkByName(string nodeName);
        IReadOnlyCollection<INode> Links { get; }
    }
}
