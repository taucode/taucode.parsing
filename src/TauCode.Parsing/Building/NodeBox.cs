using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Nodes;

namespace TauCode.Parsing.Building
{
    public class NodeBox
    {
        private readonly INode _node;
        private readonly List<string> _links;
        private bool _linksRequested;

        public NodeBox(INode node, IEnumerable<string> links = null)
        {
            _node = node ?? throw new ArgumentNullException(nameof(node));
            _links = (links ?? new List<string>()).ToList();

            if (node is FallbackNode && _links.Any())
            {
                throw new NotImplementedException(); // an error - can't add links to fallback node (todo)
            }
        }

        public INode GetNode() => _node;
        public IReadOnlyList<string> Links => _links;

        public void RequestLink(NodeBox to)
        {
            if (_node is FallbackNode)
            {
                return; // won't add any links to fallback node.
            }

            if (to == null)
            {
                throw new ArgumentNullException(nameof(to));
            }

            if (_linksRequested)
            {
                throw new InvalidOperationException("Cannot request link - links already were requested.");
            }

            if (_links.Any())
            {
                foreach (var link in _links)
                {
                    if (link == "NEXT")
                    {
                        _node.EstablishLink(to._node);
                    }
                    else
                    {
                        _node.ClaimLink(link);
                    }
                }
            }
            else
            {
                _node.EstablishLink(to._node);
            }

            _linksRequested = true;
        }

        public void DemandLink(NodeBox to)
        {
            if (_linksRequested)
            {
                throw new InvalidOperationException("Cannot demand link - links already were requested.");
            }

            if (_links.Any())
            {
                throw new InvalidOperationException("Cannot demand link if own links are present.");
            }

            _node.EstablishLink(to._node);
        }
    }
}
