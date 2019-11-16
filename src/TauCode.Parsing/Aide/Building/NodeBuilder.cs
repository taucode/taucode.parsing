using System;
using TauCode.Parsing.Aide.Results;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Aide.Building
{
    public class NodeBuilder
    {
        private readonly INodeFamily _nodeFamily;

        public NodeBuilder(TokenResult source, INodeFamily nodeFamily)
        {
            this.Source = source;
            _nodeFamily = nodeFamily;
        }

        public TokenResult Source { get; }
        public INode Node { get; private set; }

        public void Build()
        {
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
            else
            {
                throw new NotImplementedException();
            }

            this.Node = node;
        }
    }
}
