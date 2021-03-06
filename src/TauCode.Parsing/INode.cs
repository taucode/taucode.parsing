﻿using System.Collections.Generic;

namespace TauCode.Parsing
{
    public interface INode
    {
        INodeFamily Family { get; }
        string Name { get; }
        bool AcceptsToken(IToken token, IResultAccumulator resultAccumulator);
        void Act(IToken token, IResultAccumulator resultAccumulator);
        void EstablishLink(INode node);
        void ClaimLink(string nodeName);
        IReadOnlyCollection<INode> EstablishedLinks { get; }
        IReadOnlyCollection<string> ClaimedLinkNames { get; }
        IReadOnlyCollection<INode> ResolveLinks();
        IDictionary<string, string> Properties { get; }
    }
}
