using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Extensions;

namespace TauCode.Parsing
{
    public class NodeFamily : INodeFamily
    {
        #region Fields

        private readonly IDictionary<string, INode> _namedNodes;
        private readonly HashSet<INode> _nodes;

        #endregion

        #region Constructor

        public NodeFamily(string name)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));

            _namedNodes = new Dictionary<string, INode>();
            _nodes = new HashSet<INode>();
        }

        #endregion

        #region Internal

        public void RegisterNode(INode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            if (node.Name != null)
            {
                if (_namedNodes.ContainsKey(node.Name))
                {
                    throw new InvalidOperationException($"Node with name '{node.Name}' already exists in this family.");
                }

                _namedNodes.Add(node.Name, node);
            }

            _nodes.Add(node);
        }

        #endregion

        #region INodeFamily Members

        public string Name { get; }

        public INode GetNode(string nodeName)
        {
            if (nodeName == null)
            {
                throw new ArgumentNullException(nameof(nodeName));
            }

            var node = _namedNodes.GetOrDefault(nodeName) ?? throw new KeyNotFoundException($"Node not found: '{nodeName}'.");
            return node;
        }

        public INode[] GetNodes() => _namedNodes.Values.ToArray();

        #endregion
    }
}