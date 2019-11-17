using System;
using System.Collections.Generic;
using TauCode.Parsing.Aide.Results;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Aide.Building
{
    public class NodeBuilder
    {
        private readonly INodeFamily _nodeFamily;
        private readonly List<NodeBuilder> _extraLinks;

        public NodeBuilder(INodeFamily nodeFamily, TokenResult source)
        {
            this.Source = source;
            _nodeFamily = nodeFamily;
            _extraLinks = new List<NodeBuilder>();
        }

        public NodeBuilder(INodeFamily nodeFamily, string nodeName)
        {
            this.Node = new IdleNode(nodeFamily, nodeName);
            _nodeFamily = nodeFamily;
            _extraLinks = new List<NodeBuilder>();
        }

        public TokenResult Source { get; } // todo: private?

        public INode Node { get; private set; }

        public void Build()
        {
            if (this.Node != null)
            {
                throw new NotImplementedException();
            }

            INode node;
            if (this.Source.Token is EnumToken<SyntaxElement> syntaxToken)
            {
                switch (syntaxToken.Value)
                {
                    case SyntaxElement.Identifier:
                        node = new IdentifierNode(_nodeFamily, syntaxToken.Name, null);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else if (this.Source.Token is WordToken wordToken)
            {
                node = new WordNode(_nodeFamily, wordToken.Name, null);
            }
            else
            {
                throw new NotImplementedException();
            }

            this.Node = node;
        }

        public void AddExtraLink(NodeBuilder link)
        {
            _extraLinks.Add(link);
        }
    }
}
