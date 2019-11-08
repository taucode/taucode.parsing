using System;
using TauCode.Parsing.Aide.Nodes;
using TauCode.Parsing.Aide.Parsing;
using TauCode.Parsing.Aide.Tokens;
using TauCode.Parsing.Units;
using TauCode.Parsing.Units.Impl;
using TauCode.Parsing.Units.Impl.Nodes;

namespace TauCode.Parsing.Aide
{
    public class AideParser : ParserBase
    {
        protected override IUnit BuildTree()
        {
            var end = EndNode.Instance;
            var blockDefinitionBlock = this.CreateBlockDefinitionBlock(end);

            var superBlock = new Block()
            {
                Name = "superBlock",
            };

            superBlock.Capture(blockDefinitionBlock, end);
            superBlock.Head = blockDefinitionBlock;

            superBlock.FinalizeUnit();
            return superBlock;
        }

        private IBlock CreateBlockDefinitionBlock(EndNode endNode)
        {
            INode head;
            var blockDefinitionBlock = new Block(head = new SyntaxElementAideNode(
                SyntaxElement.BeginBlock, (token, context) =>
                {
                    var blockParsingResult = new BlockDefinitionResult();
                    context.AddResult(blockParsingResult);
                })
            {
                Name = "Head of BlockDefinition Block",
            })
            {
                Name = "BlockDefinition Block",
            };

            var nameDefsBlock = this.CreateNameDefinitionsBlock((context, name) =>
            {
                var blockParsingResult = context.GetLastResult<BlockDefinitionResult>();
                blockParsingResult.Name.Add(name);
                context.Modify();
            });
            nameDefsBlock.Name = "NameDefsBlock";

            head.AddLink(nameDefsBlock);

            var contentSplitter = new Splitter();
            nameDefsBlock.GetSingleExitNode().AddLink(contentSplitter);

            // word node
            var wordNode = new WordAideNode((token, context) =>
            {
                var blockResult = context.GetLastResult<BlockDefinitionResult>();
                var wordToken = (WordAideToken)token;
                blockResult.AddUnitResult(new WordNodeResult(wordToken.Word));
                context.Modify();
            })
            {
                Name = "Word",
            };

            // identifier node
            var identifierNode = new SyntaxElementAideNode(
                SyntaxElement.Identifier,
                (token, context) =>
                {
                    var blockResult = context.GetLastResult<BlockDefinitionResult>();
                    blockResult.AddUnitResult(new IdentifierNodeResult());
                    context.Modify();
                })
            {
                Name = "Identifier",
            };

            // symbol node
            var symbolNode = new SymbolAideNode((token, context) =>
            {
                var blockResult = context.GetLastResult<BlockDefinitionResult>();
                var symbolToken = (SymbolAideToken)token;
                blockResult.AddUnitResult(new SymbolNodeResult(symbolToken.Value));
                context.Modify();
            })
            {
                Name = "Symbol",
            };

            // adding nodes to content splitter
            contentSplitter.AddWay(wordNode);
            contentSplitter.AddWay(identifierNode);
            contentSplitter.AddWay(symbolNode);

            // beforeEndBlockSplitter
            var beforeEndBlockSplitter = new Splitter();

            wordNode.AddLink(beforeEndBlockSplitter);
            identifierNode.AddLink(beforeEndBlockSplitter);
            symbolNode.AddLink(beforeEndBlockSplitter);

            // endBlock
            var endBlock = new SyntaxElementAideNode(SyntaxElement.EndBlock, (token, context) => throw new NotImplementedException());

            beforeEndBlockSplitter.AddWay(endBlock);
            beforeEndBlockSplitter.AddWay(contentSplitter);

            // adding owned nodes to block
            blockDefinitionBlock.Capture(
                nameDefsBlock,
                contentSplitter,

                wordNode,
                identifierNode,
                symbolNode,

                beforeEndBlockSplitter,
                endBlock);

            endBlock.AddLink(endNode);

            return blockDefinitionBlock;
        }

        private IBlock CreateNameDefinitionsBlock(Action<IContext, string> nameAdder)
        {
            var leftParen = new SyntaxElementAideNode(
                SyntaxElement.LeftParenthesis,
                ParsingHelper.IdleTokenProcessor)
            {
                Name = "(",
            };
            var nameRef = new NameReferenceAideNode((token, context) =>
            {
                var nameReferenceToken = (NameReferenceAideToken)token;
                var name = nameReferenceToken.ReferencedName;
                nameAdder(context, name);
            })
            {
                Name = "NameRef",
            };
            var comma = new SyntaxElementAideNode(
                SyntaxElement.Comma,
                ParsingHelper.IdleTokenProcessor)
            {
                Name = ",",
            };
            var rightParen = new SyntaxElementAideNode(
                SyntaxElement.RightParenthesis,
                ParsingHelper.IdleTokenProcessor)
            {
                Name = ")",
            };

            leftParen.AddLink(nameRef);
            nameRef.AddLink(comma);
            nameRef.AddLink(rightParen);
            comma.AddLink(nameRef);

            var block = new Block(leftParen);
            block.Capture(nameRef, comma, rightParen);

            return block;
        }
    }
}
