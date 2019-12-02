using System;
using System.Collections.Generic;
using System.Linq;

namespace TauCode.Parsing.Building
{
    public class NodeBox
    {
        private readonly INode _node;
        private readonly List<string> _links;
        private bool _linksRequested;

        public NodeBox(INode node, IEnumerable<string> links = null)
        {
            // todo checks
            _node = node ?? throw new ArgumentNullException(nameof(node));
            _links = (links ?? new List<string>()).ToList();
        }

        public INode GetNode() => _node;
        public IReadOnlyList<string> Links => _links;

        public void RequestLink(NodeBox to)
        {
            // todo checks

            if (_linksRequested)
            {
                throw new NotImplementedException(); // error.
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
                throw new NotImplementedException();
            }

            if (_links.Any())
            {
                throw new NotImplementedException(); // you may not demand link if there are '_links'
            }

            _node.EstablishLink(to._node);
        }
    }
}
