using System;
using System.Linq;
using TauCode.Parsing.Aide.Nodes;
using TauCode.Parsing.Aide.Results;
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
            var head = new SplittingNode("Node: super splitter");
            var end = EndNode.Instance;
            var blockDefinitionBlock = this.CreateBlockDefinitionBlock(end);
            var cloneBlockBlock = this.CreateCloneBlockBlock(end);

            var superBlock = new Block(head, "superBlock");
            head.AddLink(blockDefinitionBlock);
            head.AddLink(cloneBlockBlock);

            superBlock.Capture(
                blockDefinitionBlock,
                cloneBlockBlock,
                end);

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
                        var blockDefinitionResult = new BlockDefinitionResult();
                        context.AddResult(blockDefinitionResult);
                    },
                    "Node: BlockDefinition"),
                "Block: BlockDefinition Block");

            var nameRefsBlock = this.CreateNameReferencesInParenthesesBlock((context, name) =>
            {
                var blockDefinitionResult = context.GetLastResult<BlockDefinitionResult>();
                blockDefinitionResult.Arguments.Add(name);
                context.Modify();
            });
            nameRefsBlock.Name = "Block: Name Refs";

            head.AddLink(nameRefsBlock);

            var blockContentBlock = this.CreateBlockContentBlock(
                out var outputSplitter,
                out var optionalInputWrapper,
                out var optionalOutputWrapper,
                out var alternativesInputWrapper,
                out var alternativesOutputWrapper);

            nameRefsBlock.GetSingleExitNode().AddLink(blockContentBlock);

            // deal with optional
            var optionalBlock = this.CreateOptionalBlock(
                out var blockInputNodeWrapperForOptional,
                out var blockOutputNodeWrapperForOptional);

            optionalInputWrapper.InternalNode = (INode)optionalBlock.Head;
            optionalOutputWrapper.InternalNode = optionalBlock.GetSingleExitNode();

            blockInputNodeWrapperForOptional.InternalNode = (INode)blockContentBlock.Head;
            blockOutputNodeWrapperForOptional.InternalNode =
                (INode)blockContentBlock.Owned.Single(x => x.Name == "Node: output splitter of block content");

            // deal with alternatives
            var alternativesBlock = this.CreateAlternativesBlock(
                out var blockInputNodeWrapperForAlternatives,
                out var blockOutputNodeWrapperForAlternatives);

            alternativesInputWrapper.InternalNode = (INode)alternativesBlock.Head;
            alternativesOutputWrapper.InternalNode = alternativesBlock.GetSingleExitNode();

            blockInputNodeWrapperForAlternatives.InternalNode = (INode)blockContentBlock.Head;
            blockOutputNodeWrapperForAlternatives.InternalNode =
                (INode)blockContentBlock.Owned.Single(x => x.Name == "Node: output splitter of block content");

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
                optionalBlock,
                alternativesBlock,
                endBlockNode);

            endBlockNode.AddLink(endNode);

            return blockDefinitionBlock;
        }

        private IBlock CreateCloneBlockBlock(EndNode endNode)
        {
            INode head;
            var block = new Block(
                head = new SyntaxElementAideNode(
                    SyntaxElement.CloneBlock,
                    (token, context) =>
                    {
                        var cloneResult = new CloneBlockResult();
                        context.AddResult(cloneResult);
                    },
                    "Node: CloneBlock"),
                "Block: CloneBlock Block");

            var nameRefsBlock = this.CreateNameReferencesInParenthesesBlock((context, name) =>
            {
                var cloneBlockResult = context.GetLastResult<CloneBlockResult>();
                cloneBlockResult.Arguments.Add(name);
                context.Modify();
            });
            nameRefsBlock.Name = "Block: Name Refs for Clone Block block";

            head.AddLink(nameRefsBlock);
            nameRefsBlock.GetSingleExitNode().AddLink(endNode);

            // adding owned nodes to block
            block.Capture(nameRefsBlock);

            return block;
        }

        private IBlock CreateBlockContentBlock(
            out SplittingNode outputSplitter,
            out INodeWrapper optionalInputWrapper,
            out INodeWrapper optionalOutputWrapper,
            out INodeWrapper alternativesInputWrapper,
            out INodeWrapper alternativesOutputWrapper)
        {
            var inputSplitter = new SplittingNode("Node: starting splitter of block content");
            var block = new Block(inputSplitter, "Block: Block content");

            // word node
            var wordNode = new WordAideNode(
                (token, context) =>
                {
                    var content = context.GetCurrentContent();
                    var wordToken = (WordAideToken)token;
                    content.AddUnitResult(new WordNodeResult(wordToken.Word, wordToken.Name));
                    context.Modify();
                },
                "Node: Word within block");

            // identifier node
            var identifierNode = new SyntaxElementAideNode(
                SyntaxElement.Identifier,
                (token, context) =>
                {
                    var content = context.GetCurrentContent();
                    content.AddUnitResult(new IdentifierNodeResult(token.GetAideTokenName()));
                    context.Modify();
                },
                "Node: Identifier within block");

            // symbol node
            var symbolNode = new SymbolAideNode(
                (token, context) =>
                {
                    var content = context.GetCurrentContent();
                    var symbolToken = (SymbolAideToken)token;
                    content.AddUnitResult(new SymbolNodeResult(symbolToken.Value, token.GetAideTokenName()));
                    context.Modify();
                },
                "Node: Symbol within block");

            // block node
            var blockNode = new SyntaxElementAideNode(
                SyntaxElement.BlockReference,
                (token, context) =>
                {
                    var content = context.GetCurrentContent();
                    content.AddUnitResult(new BlockReferenceResult(token.GetAideTokenName()));
                    context.Modify();
                },
                "Node: Block Reference");

            // link node
            var linkBlock = this.CreateLinkBlock(out var linkOutNode);

            // optional wrappers
            optionalInputWrapper = new NodeWrapper("Node Wrapper: optional Input Wrapper");
            optionalOutputWrapper = new NodeWrapper("Node Wrapper: optional Output Wrapper");

            // alternatives wrappers
            alternativesInputWrapper = new NodeWrapper("Node Wrapper: alternatives Input Wrapper");
            alternativesOutputWrapper = new NodeWrapper("Node Wrapper: alternatives Output Wrapper");

            // adding nodes to content splitter
            inputSplitter.AddLink(wordNode);
            inputSplitter.AddLink(identifierNode);
            inputSplitter.AddLink(symbolNode);
            inputSplitter.AddLink(blockNode);
            inputSplitter.AddLink(linkBlock);
            inputSplitter.AddLink(optionalInputWrapper);
            inputSplitter.AddLink(alternativesInputWrapper);

            // beforeEndBlockSplitter
            outputSplitter = new SplittingNode("Node: output splitter of block content");

            wordNode.AddLink(outputSplitter);
            identifierNode.AddLink(outputSplitter);
            symbolNode.AddLink(outputSplitter);
            blockNode.AddLink(outputSplitter);
            linkOutNode.AddLink(outputSplitter);
            optionalOutputWrapper.AddDeferredLink(outputSplitter);
            alternativesOutputWrapper.AddDeferredLink(outputSplitter);

            outputSplitter.AddLink(inputSplitter);

            block.Capture(
                wordNode,
                identifierNode,
                symbolNode,
                blockNode,
                linkBlock,
                optionalInputWrapper,
                optionalOutputWrapper,
                alternativesInputWrapper,
                alternativesOutputWrapper,
                outputSplitter);

            return block;
        }

        private IBlock CreateLinkBlock(out INode outNode)
        {
            var linkNode = new SyntaxElementAideNode(
                SyntaxElement.Link,
                (token, context) =>
                {
                    var content = context.GetCurrentContent();
                    content.AddUnitResult(new LinkResult(token.GetAideTokenName()));
                    context.Modify();
                },
                "Node: Link (head of 'Link' block)");

            var nameRefs = this.CreateNameReferencesInParenthesesBlock((context, s) =>
            {
                var content = context.GetCurrentContent();
                var linkResult = (LinkResult)content.GetLastUnitResult();
                linkResult.Arguments.Add(s);
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
                (token, context) =>
                {
                    var content = context.GetCurrentContent();
                    var optionalResult = new OptionalResult(token.GetAideTokenName());
                    content.AddUnitResult(optionalResult);
                    context.Modify();
                },
                "Node: [ of optional");

            var block = new Block(head, "Block: optional");
            blockInputNodeWrapper = new NodeWrapper("Node Wrapper: block input of optional");
            blockOutputNodeWrapper = new NodeWrapper("Node Wrapper: block output of optional");

            var closer = new SyntaxElementAideNode(
                SyntaxElement.RightBracket,
                ParsingHelper.IdleTokenProcessor,
                "Node: ] of optional");

            head.AddLink(blockInputNodeWrapper);
            blockOutputNodeWrapper.AddDeferredLink(closer);

            block.Capture(
                blockInputNodeWrapper,
                blockOutputNodeWrapper,
                closer);

            return block;
        }

        private IBlock CreateAlternativesBlock(
            out INodeWrapper blockInputNodeWrapper,
            out INodeWrapper blockOutputNodeWrapper)
        {
            var head = new SyntaxElementAideNode(
                SyntaxElement.LeftCurlyBracket,
                (token, context) =>
                {
                    var content = context.GetCurrentContent();
                    var alternativesResult = new AlternativesResult(token.GetAideTokenName());
                    content.AddUnitResult(alternativesResult);
                    context.Modify();
                },
                "Node: { of alternatives");

            var block = new Block(head, "Block: alternatives");

            blockInputNodeWrapper = new NodeWrapper("Node Wrapper: block input of alternatives");
            blockOutputNodeWrapper = new NodeWrapper("Node Wrapper: block output of alternatives");

            var verticalBar = new SyntaxElementAideNode(
                SyntaxElement.VerticalBar,
                (token, context) =>
                {
                    var content = context.GetCurrentContent();
                    var alternativesResult = (AlternativesResult)content.Owner;
                    alternativesResult.AddAlternative();
                    context.Modify();
                },
                "Node: | of alternatives");

            var closer = new SyntaxElementAideNode(
                SyntaxElement.RightCurlyBracket,
                ParsingHelper.IdleTokenProcessor,
                "Node: } of optional");

            head.AddLink(blockInputNodeWrapper);
            blockOutputNodeWrapper.AddDeferredLink(verticalBar);
            verticalBar.AddLink(blockInputNodeWrapper);
            blockOutputNodeWrapper.AddDeferredLink(closer);

            block.Capture(
                blockInputNodeWrapper,
                blockOutputNodeWrapper,
                verticalBar,
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
