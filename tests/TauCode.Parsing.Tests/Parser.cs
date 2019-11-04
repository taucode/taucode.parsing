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
            var nodeTableName = new IdentifierNodeParsingUnit((token, context) => context.Add(
                "table", 
                new
                {
                    Name = ((WordToken)token).Word,
                    Columns = new List<dynamic>(),
                }));
            var nodeLeftParen = new SymbolNodeParsingUnit('(', ParsingHelper.IdleTokenProcessor);

            nodeCreate.AddNextUnit(nodeTable);
            nodeTable.AddNextUnit(nodeTableName);
            nodeTableName.AddNextUnit(nodeLeftParen);

            createTableBlock.Enlist(nodeTable, nodeTableName, nodeLeftParen);

            // <column_definition>
            var columnName = new IdentifierNodeParsingUnit((token, context) => context.Add("column", new { Name = ((WordToken)token).Word }));

            var columnType = new IdentifierNodeParsingUnit((token, context) => context.Update("column", new { Type = ((WordToken)token).Word }));
            columnName.AddNextUnit(columnType);

            var columnDefinition = new BlockParsingUnit(columnName);
            columnDefinition.Enlist(columnType);

            nodeLeftParen.AddNextUnit(columnDefinition);

            // ',' and ')'
            var columnComma = new SymbolNodeParsingUnit(',', (token, context) =>
            {
                var column = context.Get("column");
                var table = context.Get("table");
                table.Columns.Add(column);
                context.Remove("column");
            });
            columnComma.AddNextUnit(columnDefinition);

            columnType.AddNextUnit(columnComma);

            var rightParen = new SymbolNodeParsingUnit(')', (token, context) =>
            {
                var column = context.Get("column");
                var table = context.Get("table");
                table.Columns.Add(column);
                context.Remove("column");
            });
            columnType.AddNextUnit(rightParen);

            // end
            rightParen.AddNextUnit(EndNodeParsingUnit.Instance);

            // super-block.
            var superBlock = new BlockParsingUnit(createTableBlock);
            superBlock.Enlist(createTableBlock, columnDefinition, columnComma, rightParen);


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
