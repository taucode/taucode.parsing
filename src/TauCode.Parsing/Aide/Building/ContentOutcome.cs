using System;
using System.Collections.Generic;

namespace TauCode.Parsing.Aide.Building
{
    public class ContentOutcome
    {
        public ContentOutcome()
        {
            this.Nodes = new List<NodeBuilder>();
        }

        public NodeBuilder First { get; private set; }

        public List<NodeBuilder> Nodes { get; }

        public void AddNode(NodeBuilder nodeBuilder)
        {
            // todo checks
            this.Nodes.Add(nodeBuilder);
            if (this.First == null)
            {
                this.First = nodeBuilder;
            }
        }

        public void InitLinks()
        {
            for (var i = 0; i < this.Nodes.Count; i++)
            {
                throw new NotImplementedException();
            }
        }
    }
}
