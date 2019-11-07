using TauCode.Parsing.Aide.Building;
using TauCode.Parsing.Aide.Nodes;
using TauCode.Parsing.ParsingUnits;
using TauCode.Parsing.ParsingUnits.Impl;
using TauCode.Parsing.ParsingUnits.Impl.Nodes;

namespace TauCode.Parsing.Aide
{
    public class AideParser : ParserBase
    {
        protected override IParsingUnit BuildTree()
        {
            IParsingNode head;
            var beginBlock = new ParsingBlock(head = new SyntaxElementAideNode(SyntaxElement.BeginBlock, BeginBlock));

            var leftParen = new SyntaxElementAideNode(SyntaxElement.LeftParenthesis, ParsingHelper.IdleTokenProcessor);


            var end = EndParsingNode.Instance;
            head.AddLink(end);

            beginBlock.FinalizeUnit();
            return head;
        }

        private static void BeginBlock(IToken token, IParsingContext parsingContext)
        {
            var blockBuilder = new BlockBuilder();
            parsingContext.AddResult(blockBuilder);
        }
    }
}
