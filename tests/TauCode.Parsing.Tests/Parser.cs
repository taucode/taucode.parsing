using System;
using System.Collections.Generic;
using TauCode.Parsing.ParsingUnits;
using TauCode.Parsing.Tests.Tokens;
using TauCode.Parsing.Tests.Units;

namespace TauCode.Parsing.Tests
{
    public class Parser : IParser
    {
        private readonly IParsingUnit _head;

        public Parser()
        {
            _head = this.BuildTree();
        }

        private IParsingUnit BuildTree()
        {
            // CREATE TABLE (
            var nodeCreate = new WordNodeParsingUnit("CREATE", ParsingHelper.IdleTokenProcessor);
            var createTableBlock = new BlockParsingUnit(nodeCreate);
            var nodeTable = new WordNodeParsingUnit("TABLE", ParsingHelper.IdleTokenProcessor);
            var nodeTableName = new IdentifierNodeParsingUnit((token, context) => context.Push("table", new { Name = ((WordToken)token).Word }));
            var nodeLeftParen = new SymbolNodeParsingUnit('(', ParsingHelper.IdleTokenProcessor);

            nodeCreate.AddNextNode(nodeTable);
            nodeTable.AddNextNode(nodeTableName);
            nodeTableName.AddNextNode(nodeLeftParen);

            return createTableBlock;
        }

        public IParsingContext Parse(IEnumerable<IToken> tokens)
        {
            var context = new ParsingContext();
            var stream = new TokenStream(tokens);

            do
            {
                var result = _head.Process(stream, context);
                throw new NotImplementedException();

            } while (true);
        }
    }
}
