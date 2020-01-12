//using System;
//using System.Collections.Generic;
//using System.Linq;
//using TauCode.Extensions;

//namespace TauCode.Parsing.Old
//{
//    public class OldNodeFamily : INodeFamily
//    {
//        #region Fields

//        private readonly IDictionary<string, INode> _namedNodes;
//        private readonly HashSet<INode> _nodes;

//        #endregion

//        #region Constructor

//        public OldNodeFamily(string name)
//        {
//            this.Name = name ?? throw new ArgumentNullException(nameof(name));

//            _namedNodes = new Dictionary<string, INode>();
//            _nodes = new HashSet<INode>();
//        }

//        #endregion

//        #region Internal



//        // todo: use this!
//        internal void OldRegisterNode(INode node)
//        {
//            if (node == null)
//            {
//                throw new ArgumentNullException(nameof(node));
//            }

//            if (node.Name != null)
//            {
//                _namedNodes.Add(node.Name, node);
//            }

//            _nodes.Add(node);
//        }

//        #endregion

//        #region INodeFamily Members

//        public string Name { get; }

//        public void RegisterNode(INode node)
//        {
//            throw new NotImplementedException("todo: this is a stub!");
//        }

//        public INode GetNode(string nodeName)
//        {
//            if (nodeName == null)
//            {
//                throw new ArgumentNullException(nameof(nodeName));
//            }

//            var node = _namedNodes.GetOrDefault(nodeName) ?? throw new KeyNotFoundException($"Node not found: '{nodeName}'.");
//            return node;
//        }

//        public INode[] GetNodes() => _namedNodes.Values.ToArray();

//        #endregion
//    }
//}


// todo clean up