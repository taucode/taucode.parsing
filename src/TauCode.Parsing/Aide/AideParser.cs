using System;
using TauCode.Parsing.Aide.Nodes;
using TauCode.Parsing.Aide.Parsing;
using TauCode.Parsing.Aide.Tokens;
using TauCode.Parsing.ParsingUnits;
using TauCode.Parsing.ParsingUnits.Impl;
using TauCode.Parsing.ParsingUnits.Impl.Nodes;

namespace TauCode.Parsing.Aide
{
    public class AideParser : ParserBase
    {
        protected override IParsingUnit BuildTree()
        {
            var end = EndParsingNode.Instance;
            var blockDefinitionBlock = this.CreateBlockDefinitionBlock(end);

            //beginBlock.Add(nameDefsBlock);
            //head.AddLink(nameDefsBlock);


            //nameDefsBlock.GetSingleExitNode().AddLink(end);

            IParsingSplitter root;
            var superBlock = new ParsingBlock(root = new ParsingSplitter
            {
                Name = "root",
            })
            {
                Name = "superBlock",
            };

            root.AddWay(blockDefinitionBlock);
            root.AddWay(end);

            superBlock.Capture(blockDefinitionBlock, end);

            superBlock.FinalizeUnit();
            return superBlock;
        }

        private IParsingBlock CreateBlockDefinitionBlock(EndParsingNode endNode)
        {
            IParsingNode head;
            var blockDefinitionBlock = new ParsingBlock(head = new SyntaxElementAideNode(
                SyntaxElement.BeginBlock, (token, context) =>
                {
                    var blockParsingResult = new BlockParsingResult();
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
                var blockParsingResult = context.GetLastResult<BlockParsingResult>();
                blockParsingResult.Name.Add(name);
            });
            nameDefsBlock.Name = "NameDefsBlock";

            head.AddLink(nameDefsBlock);

            var contentSplitter = new ParsingSplitter();
            nameDefsBlock.GetSingleExitNode().AddLink(contentSplitter);

            // word node
            var wordNode = new WordAideNode((token, context) =>
            {
                var blockResult = context.GetLastResult<BlockParsingResult>();
                var wordToken = (WordAideToken)token;
                blockResult.AddUnitResult(new WordNodeResult(wordToken.Word));
                throw new NotImplementedException();
            });

            // adding nodes to content splitter
            contentSplitter.AddWay(wordNode);

            // beforeEndBlockSplitter
            var beforeEndBlockSplitter = new ParsingSplitter();


            wordNode.AddLink(beforeEndBlockSplitter);

            // endBlock
            var endBlock = new SyntaxElementAideNode(SyntaxElement.EndBlock, (token, context) => throw new NotImplementedException());

            beforeEndBlockSplitter.AddWay(endBlock);
            beforeEndBlockSplitter.AddWay(contentSplitter);

            // adding owned nodes to block
            blockDefinitionBlock.Capture(
                nameDefsBlock,
                contentSplitter,
                wordNode,
                beforeEndBlockSplitter,
                endBlock);

            endBlock.AddLink(endNode);

            return blockDefinitionBlock;
        }

        private IParsingBlock CreateNameDefinitionsBlock(Action<IParsingContext, string> nameAdder)
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
                //var block
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

            var block = new ParsingBlock(leftParen);
            block.Capture(nameRef, comma, rightParen);

            return block;
        }
    }
}
