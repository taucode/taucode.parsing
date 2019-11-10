using System;
using System.Linq;
using TauCode.Parsing.Aide.Nodes;
using TauCode.Parsing.Aide.Parsing;
using TauCode.Parsing.Aide.Tokens;
using TauCode.Parsing.Units;
using TauCode.Parsing.Units.Impl;
using TauCode.Parsing.Units.Impl.Nodes;

namespace TauCode.Parsing.Aide
{
    // todo: cleanup
    public class AideParser : ParserBase
    {
        protected override IUnit BuildTree()
        {
            var end = EndNode.Instance;
            var blockDefinitionBlock = this.CreateBlockDefinitionBlock(end);

            var superBlock = new Block("superBlock");

            superBlock.Capture(blockDefinitionBlock, end);
            superBlock.Head = blockDefinitionBlock;

            superBlock.FinalizeUnit();
            return superBlock;
        }

        private IBlock CreateBlockDefinitionBlock(EndNode endNode)
        {
            INode head;
            var blockDefinitionBlock = new Block(
                head = new SyntaxElementAideNode(
                    SyntaxElement.BeginBlockDefinition,
                    (token, context) =>
                    {
                        var blockParsingResult = new BlockContentResult();
                        context.AddResult(blockParsingResult);
                    },
                    "Node: BlockDefinition"),
                "Block: BlockDefinition Block");

            var nameRefsBlock = this.CreateNameReferencesInParenthesesBlock((context, name) =>
            {
                var blockDefinitionResult = context.GetLastResult<BlockContentResult>();
                blockDefinitionResult.Arguments.Add(name);
                context.Modify();
            });
            nameRefsBlock.Name = "Block: Name Refs";

            head.AddLink(nameRefsBlock);

            var blockContentBlock = this.CreateBlockContentBlock(
                out var outputSplitter,
                out var optionalInputWrapper,
                out var optionalOutputWrapper);

            var optionalBlock = this.CreateOptionalBlock(out var blockInputNodeWrapper, out var blockOutputNodeWrapper);

            optionalInputWrapper.InternalNode = (INode)optionalBlock.Head;
            optionalOutputWrapper.InternalNode = optionalBlock.GetSingleExitNode();

            blockInputNodeWrapper.InternalNode = (INode)blockContentBlock.Head;
            blockOutputNodeWrapper.InternalNode =
                (INode)blockContentBlock.Owned.Single(x => x.Name == "Node: output splitter of block content");

            nameRefsBlock.GetSingleExitNode().AddLink(blockContentBlock);

            //var dummyNode1 = new SyntaxElementAideNode(
            //    SyntaxElement.NotExisting, 
            //    (token, context) => throw new NotImplementedException(),
            //    "Node: dummyNode1");

            //var dummyNode2 = new SyntaxElementAideNode(
            //    SyntaxElement.NotExisting, 
            //    (token, context) => throw new NotImplementedException(),
            //    "Node: dummyNode2");

            //optionalInputWrapper.InternalNode = dummyNode1;
            //optionalOutputWrapper.InternalNode = dummyNode2;

            //optionalInputWrapper.AddDeferredLink(optionalOutputWrapper);

            // endBlockDefinitionNode
            var endBlockNode = new SyntaxElementAideNode(
                SyntaxElement.EndBlockDefinition,
                ParsingHelper.IdleTokenProcessor,
                "Node: end block definition");

            outputSplitter.AddLink(endBlockNode);

            // adding owned nodes to block
            blockDefinitionBlock.Capture(
                nameRefsBlock,
                blockContentBlock,
                //dummyNode1,
                //dummyNode2,
                optionalBlock,
                endBlockNode);

            endBlockNode.AddLink(endNode);

            return blockDefinitionBlock;
        }

        private IBlock CreateBlockContentBlock(
            out SplittingNode outputSplitter,
            out INodeWrapper optionalInputWrapper,
            out INodeWrapper optionalOutputWrapper)
        {
            var inputSplitter = new SplittingNode("Node: starting splitter of block content");
            var block = new Block(inputSplitter, "Block: Block content");

            // word node
            var wordNode = new WordAideNode(
                (token, context) =>
                {
                    var blockDefinitionResult = context.GetLastResult<BlockContentResult>();
                    var wordToken = (WordAideToken)token;
                    blockDefinitionResult.AddUnitResult(new WordNodeResult(wordToken.Word, wordToken.Name));
                    context.Modify();
                },
                "Node: Word within block");

            // identifier node
            var identifierNode = new SyntaxElementAideNode(
                SyntaxElement.Identifier,
                (token, context) =>
                {
                    var blockDefinitionResult = context.GetLastResult<BlockContentResult>();
                    blockDefinitionResult.AddUnitResult(new IdentifierNodeResult(null));
                    context.Modify();
                },
                "Node: Identifier within block");

            // symbol node
            var symbolNode = new SymbolAideNode(
                (token, context) =>
                {
                    var blockDefinitionResult = context.GetLastResult<BlockContentResult>();
                    var symbolToken = (SymbolAideToken)token;
                    blockDefinitionResult.AddUnitResult(new SymbolNodeResult(symbolToken.Value, null));
                    context.Modify();
                },
                "Node: Symbol within block");

            // block node
            var blockNode = new SyntaxElementAideNode(
                SyntaxElement.BlockReference,
                (token, context) =>
                {
                    var blockDefinitionResult = context.GetLastResult<BlockContentResult>();
                    var syntaxElementAideToken = (SyntaxElementAideToken)token;
                    blockDefinitionResult.AddUnitResult(new BlockReferenceResult(syntaxElementAideToken.Name));
                    context.Modify();
                },
                "Node: Block Reference");

            // link node
            var linkBlock = this.CreateLinkBlock(out var linkOutNode);

            // optional wrapper
            optionalInputWrapper = new NodeWrapper("Node Wrapper: optional Input Wrapper");
            optionalOutputWrapper = new NodeWrapper("Node Wrapper: optional Output Wrapper");

            // adding nodes to content splitter
            inputSplitter.AddLink(wordNode);
            inputSplitter.AddLink(identifierNode);
            inputSplitter.AddLink(symbolNode);
            inputSplitter.AddLink(blockNode);
            inputSplitter.AddLink(linkBlock);
            inputSplitter.AddLink(optionalInputWrapper);

            // beforeEndBlockSplitter
            outputSplitter = new SplittingNode("Node: output splitter of block content");

            wordNode.AddLink(outputSplitter);
            identifierNode.AddLink(outputSplitter);
            symbolNode.AddLink(outputSplitter);
            blockNode.AddLink(outputSplitter);
            linkOutNode.AddLink(outputSplitter);
            optionalOutputWrapper.AddDeferredLink(outputSplitter);

            outputSplitter.AddLink(inputSplitter);

            block.Capture(
                wordNode,
                identifierNode,
                symbolNode,
                blockNode,
                linkBlock,
                optionalInputWrapper,
                optionalOutputWrapper,
                outputSplitter);

            return block;
        }

        private IBlock CreateLinkBlock(out INode outNode)
        {
            var linkNode = new SyntaxElementAideNode(
                SyntaxElement.Link,
                (token, context) =>
                {
                    var blockDefinitionResult = context.GetLastResult<BlockContentResult>();
                    var syntaxElementAideToken = (SyntaxElementAideToken)token;
                    blockDefinitionResult.AddUnitResult(new LinkResult(null)); // todo
                    context.Modify();
                },
                "Node: Link (head of 'Link' block)");

            var nameRefs = this.CreateNameReferencesInParenthesesBlock((context, s) =>
            {
                var blockDefinitionResult = context.GetLastResult<BlockContentResult>();
                var linkResult = (LinkResult)blockDefinitionResult.GetLastUnitResult<LinkResult>();
                linkResult.AddArgument(s);
                context.Modify();
            });

            linkNode.AddLink(nameRefs);

            var block = new Block(linkNode, "Block: Link");

            block.Capture(nameRefs);
            outNode = nameRefs.GetSingleExitNode();
            return block;
        }

        private IBlock CreateOptionalBlock(
            out INodeWrapper blockInputNodeWrapper,
            out INodeWrapper blockOutputNodeWrapper)
        {
            var head = new SyntaxElementAideNode(
                SyntaxElement.LeftBracket,
                (token, context) => throw new NotImplementedException(),
                "Node: [ of optional");

            var block = new Block(head, "Block: optional");
            blockInputNodeWrapper = new NodeWrapper("Node Wrapper: block input of optional");
            blockOutputNodeWrapper = new NodeWrapper("Node Wrapper: block output of optional");

            var closer = new SyntaxElementAideNode(
                SyntaxElement.RightBracket,
                (token, context) => throw new NotImplementedException(),
                "Node: ] of optional");

            head.AddLink(blockInputNodeWrapper);
            blockOutputNodeWrapper.AddDeferredLink(closer);

            block.Capture(
                blockInputNodeWrapper,
                blockOutputNodeWrapper,
                closer);

            return block;
        }

        private IBlock CreateNameReferencesInParenthesesBlock(Action<IContext, string> nameAdder)
        {
            var leftParen = new SyntaxElementAideNode(
                SyntaxElement.LeftParenthesis,
                ParsingHelper.IdleTokenProcessor,
                "Node: ( of name references");
            var nameRef = new NameReferenceAideNode(
                (token, context) =>
                {
                    var nameReferenceToken = (NameReferenceAideToken)token;
                    var name = nameReferenceToken.ReferencedName;
                    nameAdder(context, name);
                },
                "Node: name reference");
            var comma = new SyntaxElementAideNode(
                SyntaxElement.Comma,
                ParsingHelper.IdleTokenProcessor,
                "Node: comma of name references");
            var rightParen = new SyntaxElementAideNode(
                SyntaxElement.RightParenthesis,
                ParsingHelper.IdleTokenProcessor,
                "Node: ) of name references");

            leftParen.AddLink(nameRef);
            nameRef.AddLink(comma);
            nameRef.AddLink(rightParen);
            comma.AddLink(nameRef);

            var block = new Block(leftParen, "Block: name references");
            block.Capture(nameRef, comma, rightParen);

            return block;
        }
    }
}
