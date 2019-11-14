using System.Collections.Generic;

namespace TauCode.Parsing
{
    public interface INode2
    {
        INodeFamily Family { get; }
        string Name { get; }
        InquireResult Inquire(IToken token);
        void Act(IToken token, IResultAccumulator resultAccumulator);
        void AddLink(INode2 node);
        void AddLinkByName(string nodeName);
        IReadOnlyCollection<INode2> Links { get; }
    }
}
