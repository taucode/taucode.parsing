using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Aide.Results;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Aide.Building
{
    public class NodeBuilder
    {
        private readonly List<NodeBuilder> _explicitLinks;
        private readonly List<string> _linksToClaim;
        private readonly List<string> _arguments;

        public NodeBuilder(INodeFamily nodeFamily, TokenResult tokenResult)
            : this(BuildNode(nodeFamily, tokenResult), tokenResult.Arguments)
        {
        }

        public NodeBuilder(INode node, IEnumerable<string> arguments)
        {
            this.Node = node ?? throw new ArgumentNullException(nameof(node));
            _explicitLinks = new List<NodeBuilder>();
            _linksToClaim = new List<string>();
            _arguments = new List<string>();
            if (arguments != null)
            {
                _arguments.AddRange(arguments);
            }
        }

        public List<string> Arguments => _arguments;

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

                    case SyntaxElement.Idle:
                        node = new IdleNode(nodeFamily, syntaxToken.Name);
                        break;

                    case SyntaxElement.Word:
                        node = new WordNode(nodeFamily, syntaxToken.Name, null);
                        break;

                    case SyntaxElement.Integer:
                        node = new IntegerNode(nodeFamily, syntaxToken.Name, null);
                        break;

                    case SyntaxElement.String:
                        node = new StringNode(nodeFamily, syntaxToken.Name, null);
                        break;

                    case SyntaxElement.SpecialString:
                        var @class = syntaxToken.Properties.GetOrDefault(AideHelper.AideSpecialStringClassName);
                        node = new ClassedSpecialStringNode(nodeFamily, syntaxToken.Name, null, @class);
                        break;

                    case SyntaxElement.End:
                        node = EndNode.Instance;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else if (tokenResult.Token is WordToken wordToken)
            {
                node = new ExactWordNode(nodeFamily, wordToken.Name, null, wordToken.Word);
            }
            else if (tokenResult.Token is StringToken stringToken)
            {
                var @class = stringToken.Properties.Single().Value; // todo!
                node = new ExactSpecialStringNode(nodeFamily, stringToken.Name, null, @class, stringToken.String);
            }
            else if (tokenResult.Token is SymbolToken symbolToken)
            {
                node = new ExactSymbolNode(nodeFamily, symbolToken.Name, null, symbolToken.Value);
            }
            else
            {
                throw new AideException($"Could not build node using token of type '{tokenResult.Token.GetType().FullName}'.");
            }

            return node;
        }

        public void AddExplicitLink(NodeBuilder to)
        {
            _explicitLinks.Add(to);
        }

        public void AddLinkClaim(string linkClaim)
        {
            _linksToClaim.Add(linkClaim);
        }

        public void InitNodeLinks()
        {
            foreach (var explicitLink in _explicitLinks)
            {
                this.Node.EstablishLink(explicitLink.Node);
            }

            foreach (var linkToClaim in _linksToClaim)
            {
                this.Node.ClaimLink(linkToClaim);
            }
        }
    }
}
