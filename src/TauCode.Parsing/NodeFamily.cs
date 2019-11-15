using System;
using System.Collections.Generic;

namespace TauCode.Parsing
{
    // todo: consider rid of 'address' word anywhere
    public class NodeFamily : INodeFamily
    {
        private readonly Dictionary<string, INode> _nodesByName;
        private readonly HashSet<INode> _nodes;

        public NodeFamily(string name)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));

            _nodesByName = new Dictionary<string, INode>();
            _nodes = new HashSet<INode>();
        }

        internal void RegisterNode(INode node)
        {
            // todo: checks

            _nodesByName.Add(node.Name, node);
            _nodes.Add(node);
        }

        public string Name { get; }

        public INode GetNode(string nodeName)
        {
            // todo checks
            return _nodesByName[nodeName];
        }

        public INode[] GetNodes()
        {
            throw new NotImplementedException();
        }

        public void AddLink(string fromName, string toName)
        {
            throw new NotImplementedException();
        }
    }
}
