using System.Collections.Generic;

namespace TauCode.Parsing
{
    // todo clean
    public interface INode
    {
        INodeFamily Family { get; }
        string Name { get; }
        /*InquireResult*/ bool Inquire(IToken token, IResultAccumulator resultAccumulator); // todo: rename to AcceptsToken
        void Act(IToken token, IResultAccumulator resultAccumulator);
        void EstablishLink(INode node);
        void ClaimLink(string nodeName);
        IReadOnlyCollection<INode> EstablishedLinks { get; }
        IReadOnlyCollection<string> ClaimedLinkNames { get; }
        IReadOnlyCollection<INode> ResolveLinks();
        IDictionary<string, string> Properties { get; }
    }
}
