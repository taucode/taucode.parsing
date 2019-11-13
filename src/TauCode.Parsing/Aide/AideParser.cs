using System;
using System.Linq;
using TauCode.Parsing.Aide.Results;
using TauCode.Parsing.Tokens;
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

            var superBlock = new Block(head, "superBlock");
            head.AddLink(blockDefinitionBlock);

            superBlock.Capture(blockDefinitionBlock);

            superBlock.FinalizeUnit();
            return superBlock;
        }

        private IBlock CreateBlockDefinitionBlock(EndNode endNode)
        {
            INode head;
            var blockDefinitionBlock = new Block(
                head = new ExactEnumNode<SyntaxElement>(
                    SyntaxElement.BeginBlockDefinition,
                    (token, context) =>
                    {
                        var blockDefinitionResult = new BlockDefinitionResult();
                        context.AddResult(blockDefinitionResult);
                    },
                    "Node: BlockDefinition"),
                "Block: BlockDefinition Block");

            var nameRefsBlock = this.CreateNameReferencesInParenthesesBlock(
                (context, name) =>
                {
                    var blockDefinitionResult = context.GetLastResult<BlockDefinitionResult>();
                    blockDefinitionResult.Arguments.Add(name);
                    context.Modify();
                });
            //nameRefsBlock.Name = "Block: Name Refs";

            head.AddLink(nameRefsBlock);

            var blockContentBlock = this.CreateBlockContentBlock(
                out var outputSplitter,
                out var optionalInputWrapper,
                out var optionalOutputWrapper,
                out var alternativesInputWrapper,
                out var alternativesOutputWrapper);

            var nameRefsBlockExitNode = nameRefsBlock.GetSingleExitNode();

            nameRefsBlockExitNode.AddLink(blockContentBlock);

            // deal with clone
            var cloneNode = new ExactEnumNode<SyntaxElement>(
                SyntaxElement.Clone,
                (token, context) =>
                {
                    var content = context.GetCurrentContent();
                    content.AddUnitResult(new SyntaxElementResult(SyntaxElement.Clone, null));
                    context.Modify();
                },
                "clone");
            nameRefsBlockExitNode.AddLink(cloneNode);
            
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
            var endBlockNode = new ExactEnumNode<SyntaxElement>(
                SyntaxElement.EndBlockDefinition,
                (token, context) =>
                {
                    var content = context.GetCurrentContent();
                    content.Seal();
                },
                "Node: end block definition");


            cloneNode.AddLink(endBlockNode);

            outputSplitter.AddLink(endBlockNode);

            // adding owned nodes to block
            blockDefinitionBlock.Capture(
                nameRefsBlock,
                cloneNode,
                blockContentBlock,
                optionalBlock,
                alternativesBlock,
                endBlockNode);

            endBlockNode.AddLink(endNode);

            return blockDefinitionBlock;
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
            var wordNode = new WordNode(
                (token, context) =>
                {
                    var content = context.GetCurrentContent();
                    var wordToken = (WordToken)token;
                    content.AddUnitResult(new WordNodeResult(wordToken.Word, wordToken.Name));
                    context.Modify();
                },
                "Node: Word within block");

            // identifier node
            var identifierNode = new ExactEnumNode<SyntaxElement>(
                SyntaxElement.Identifier,
                (token, context) =>
                {
                    var content = context.GetCurrentContent();
                    content.AddUnitResult(new SyntaxElementResult(SyntaxElement.Identifier, token.Name));
                    context.Modify();
                },
                "Node: Identifier within block");

            // symbol node
            var symbolNode = new SymbolNode(
                (token, context) =>
                {
                    var content = context.GetCurrentContent();
                    var symbolToken = (SymbolToken)token;
                    content.AddUnitResult(new SymbolNodeResult(symbolToken.Value, token.Name));
                    context.Modify();
                },
                "Node: Symbol within block");

            // idle node
            var idleNode = new ExactEnumNode<SyntaxElement>(
                SyntaxElement.Idle,
                (token, context) =>
                {
                    var content = context.GetCurrentContent();
                    content.AddUnitResult(new SyntaxElementResult(SyntaxElement.Idle, token.Name));
                    context.Modify();
                },
                "Node : Idle node");

            // block node
            var blockNode = new ExactEnumNode<SyntaxElement>(
                SyntaxElement.BlockReference,
                (token, context) =>
                {
                    var content = context.GetCurrentContent();
                    content.AddUnitResult(new SyntaxElementResult(SyntaxElement.BlockReference, token.Name));
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
            inputSplitter.AddLink(idleNode);
            inputSplitter.AddLink(blockNode);
            inputSplitter.AddLink(linkBlock);
            inputSplitter.AddLink(optionalInputWrapper);
            inputSplitter.AddLink(alternativesInputWrapper);

            // beforeEndBlockSplitter
            outputSplitter = new SplittingNode("Node: output splitter of block content");

            wordNode.AddLink(outputSplitter);
            identifierNode.AddLink(outputSplitter);
            symbolNode.AddLink(outputSplitter);
            idleNode.AddLink(outputSplitter);
            blockNode.AddLink(outputSplitter);
            linkOutNode.AddLink(outputSplitter);
            optionalOutputWrapper.AddDeferredLink(outputSplitter);
            alternativesOutputWrapper.AddDeferredLink(outputSplitter);

            outputSplitter.AddLink(inputSplitter);

            block.Capture(
                wordNode,
                identifierNode,
                symbolNode,
                idleNode,
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
            var linkNode = new ExactEnumNode<SyntaxElement>(
                SyntaxElement.Link,
                (token, context) =>
                {
                    var content = context.GetCurrentContent();
                    content.AddUnitResult(new SyntaxElementResult(SyntaxElement.Link, token.Name));
                    context.Modify();
                },
                "Node: Link (head of 'Link' block)");

            var nameRefs = this.CreateNameReferencesInParenthesesBlock((context, s) =>
            {
                var content = context.GetCurrentContent();
                var linkResult = (SyntaxElementResult)content.GetLastUnitResult();
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
            var head = new ExactEnumNode<SyntaxElement>(
                SyntaxElement.LeftBracket,
                (token, context) =>
                {
                    var content = context.GetCurrentContent();
                    var optionalResult = new OptionalResult(token.Name);
                    content.AddUnitResult(optionalResult);
                    context.Modify();
                },
                "Node: [ of optional");

            var block = new Block(head, "Block: optional");
            blockInputNodeWrapper = new NodeWrapper("Node Wrapper: block input of optional");
            blockOutputNodeWrapper = new NodeWrapper("Node Wrapper: block output of optional");

            var closer = new ExactEnumNode<SyntaxElement>(
                SyntaxElement.RightBracket,
                (token, context) =>
                {
                    var content = context.GetCurrentContent();
                    content.Seal();
                },
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
            var head = new ExactEnumNode<SyntaxElement>(
                SyntaxElement.LeftCurlyBracket,
                (token, context) =>
                {
                    var content = context.GetCurrentContent();
                    var alternativesResult = new AlternativesResult(token.Name);
                    content.AddUnitResult(alternativesResult);
                    context.Modify();
                },
                "Node: { of alternatives");

            var block = new Block(head, "Block: alternatives");

            blockInputNodeWrapper = new NodeWrapper("Node Wrapper: block input of alternatives");
            blockOutputNodeWrapper = new NodeWrapper("Node Wrapper: block output of alternatives");

            var verticalBar = new ExactEnumNode<SyntaxElement>(
                SyntaxElement.VerticalBar,
                (token, context) =>
                {
                    var content = context.GetCurrentContent();
                    var alternativesResult = (AlternativesResult)content.Owner;
                    alternativesResult.AddAlternative();
                    context.Modify();
                },
                "Node: | of alternatives");

            var closer = new ExactEnumNode<SyntaxElement>(
                SyntaxElement.RightCurlyBracket,
                (token, context) =>
                {
                    var content = context.GetCurrentContent();
                    content.Seal();
                },
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
            var leftParen = new ExactEnumNode<SyntaxElement>(
                SyntaxElement.LeftParenthesis,
                ParsingHelper.IdleTokenProcessor,
                "Node: ( of name references");
            var nameRef = new SpecialStringNode<AideSpecialString>(
                AideSpecialString.NameReference,
                (token, context) =>
                {
                    var nameReferenceToken = (SpecialStringToken<AideSpecialString>)token;
                    var name = nameReferenceToken.Value;
                    nameAdder(context, name);
                },
                "Node: name reference");
            var comma = new ExactEnumNode<SyntaxElement>(
                SyntaxElement.Comma,
                ParsingHelper.IdleTokenProcessor,
                "Node: comma of name references");
            var rightParen = new ExactEnumNode<SyntaxElement>(
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
