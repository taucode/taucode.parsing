using System;
using System.Collections.Generic;
using TauCode.Parsing.Aide.Results;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Aide.Building
{
    public class NodeBuilder
    {
        //private readonly INodeFamily _nodeFamily;
        private readonly List<NodeBuilder> _explicitLinks;
        private readonly List<string> _linksToClaim;

        public NodeBuilder(INodeFamily nodeFamily, TokenResult tokenResult)
            : this(BuildNode(nodeFamily, tokenResult))
        {
            // todo check args
            //this.Source = source;
            //_nodeFamily = nodeFamily;
        }

        public NodeBuilder(INode node)
        {
            // todo check args
            this.Node = node;
            _explicitLinks = new List<NodeBuilder>();
            _linksToClaim = new List<string>();
            //this.Node = new IdleNode(nodeFamily, nodeName);
            //_nodeFamily = nodeFamily;
            //_explicitLinks = new List<NodeBuilder>();
            //_linksToClaim = new List<string>();
        }

        //public TokenResult Source { get; } // todo: private?

        public INode Node { get; private set; }

        private static INode BuildNode(INodeFamily nodeFamily, TokenResult tokenResult)
        {
            INode node;
            if (tokenResult.Token is EnumToken<SyntaxElement> syntaxToken)
            {
                switch (syntaxToken.Value)
                {
                    case SyntaxElement.Identifier:
                        node = new IdentifierNode(nodeFamily, syntaxToken.Name, null);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else if (tokenResult.Token is WordToken wordToken)
            {
                node = new WordNode(nodeFamily, wordToken.Name, null);
            }
            else
            {
                throw new NotImplementedException();
            }

            return node;
        }

        //public void Build()
        //{
        //    if (this.Node != null)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    INode node;
        //    if (this.Source.Token is EnumToken<SyntaxElement> syntaxToken)
        //    {
        //        switch (syntaxToken.Value)
        //        {
        //            case SyntaxElement.Identifier:
        //                node = new IdentifierNode(_nodeFamily, syntaxToken.Name, null);
        //                break;

        //            default:
        //                throw new ArgumentOutOfRangeException();
        //        }
        //    }
        //    else if (this.Source.Token is WordToken wordToken)
        //    {
        //        node = new WordNode(_nodeFamily, wordToken.Name, null);
        //    }
        //    else
        //    {
        //        throw new NotImplementedException();
        //    }

        //    this.Node = node;
        //}

        //public void AddExtraLink(NodeBuilder link)
        //{
        //    _explicitLinks.Add(link);
        //}
    }
}
