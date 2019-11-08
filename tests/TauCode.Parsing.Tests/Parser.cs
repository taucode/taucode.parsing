using System;
using System.Collections.Generic;
using TauCode.Parsing.ParsingUnits;
using TauCode.Parsing.ParsingUnits.Impl;
using TauCode.Parsing.ParsingUnits.Impl.Nodes;
using TauCode.Parsing.Tests.Units;
using TauCode.Parsing.Tokens;

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
            var nodeCreate = new WordNode("CREATE", ParsingHelper.IdleTokenProcessor);
            var createTableBlock = new ParsingBlock(nodeCreate);
            var nodeTable = new WordNode("TABLE", ParsingHelper.IdleTokenProcessor);
            var nodeTableName = new IdentifierNode((token, context) => context.AddResult(
                //"table",
                new DynamicResult(
                new
                {
                    Name = ((WordToken)token).Word,
                    Columns = new List<dynamic>(),
                })));
            var nodeLeftParen = new SymbolNode('(', ParsingHelper.IdleTokenProcessor);

            nodeCreate.AddLink(nodeTable);
            nodeTable.AddLink(nodeTableName);
            nodeTableName.AddLink(nodeLeftParen);

            createTableBlock.Capture(nodeTable, nodeTableName, nodeLeftParen);

            // <column_definition>
            var columnName = new IdentifierNode((token, context) =>
                context.GetLastResult<dynamic>().Columns.Add(new DynamicResult(new { Name = ((WordToken)token).Word })));

            var columnType = new IdentifierNode((token, context) =>
            {
                var table = context.GetLastResult<dynamic>();
                var columns = table.Columns;
                var columnCount = columns.Count;
                var column = columns[columnCount - 1];
                var type = ((WordToken)token).Word;
                column.Type = type;
            });
            columnName.AddLink(columnType);

            var columnDefinition = new ParsingBlock(columnName);
            columnDefinition.Capture(columnType);

            nodeLeftParen.AddLink(columnDefinition);

            // ',' and ')'
            var columnComma = new SymbolNode(',', ParsingHelper.IdleTokenProcessor);
            columnComma.AddLink(columnDefinition);


            columnType.AddLink(columnComma);

            var rightParen = new SymbolNode(')', ParsingHelper.IdleTokenProcessor);
            columnType.AddLink(rightParen);

            // end
            rightParen.AddLink(EndParsingNode.Instance);

            // super-block.
            var superBlock = new ParsingBlock(createTableBlock);
            superBlock.Capture(columnDefinition, columnComma, rightParen);

            superBlock.FinalizeUnit();

            return superBlock;
        }

        public IParsingContext Parse(IEnumerable<IToken> tokens)
        {
            var context = new ParsingContext();
            var stream = new TokenStream(tokens);
            var current = _head;

            do
            {
                var result = current.Process(stream, context);
                if (result == null)
                {
                    throw new NotImplementedException();
                }
                else if (result.Count == 1)
                {
                    if (ParsingHelper.IsEndResult(result))
                    {
                        return context;
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }

                throw new NotImplementedException();

            } while (true);
        }
    }
}
