//using System;
//using System.Collections.Generic;
//using TauCode.Parsing.Tokens;
//using TauCode.Parsing.Units;
//using TauCode.Parsing.Units.Impl;
//using TauCode.Parsing.Units.Impl.Nodes;

//namespace TauCode.Parsing.Tests
//{
//    public class Parser : IParser
//    {
//        private readonly IUnit _head;

//        public Parser()
//        {
//            _head = this.BuildTree();
//        }

//        private IUnit BuildTree()
//        {
//            // CREATE TABLE (
//            var nodeCreate = new ExactWordNode("CREATE", ParsingHelper.IdleTokenProcessor, "Node: CREATE");
//            var createTableBlock = new Block(nodeCreate, "Block: CREATE TABLE");
//            var nodeTable = new ExactWordNode("TABLE", ParsingHelper.IdleTokenProcessor, "Node: TABLE");
//            var nodeTableName = new IdentifierNode(
//                (token, context) => context.AddResult(
//                    new DynamicResult(
//                        new
//                        {
//                            Name = ((WordToken)token).Word,
//                            Columns = new List<dynamic>(),

//                        })),
//                "Node: table name");
//            var nodeLeftParen = new ExactSymbolNode('(', ParsingHelper.IdleTokenProcessor, "Node: table (");

//            nodeCreate.AddLink(nodeTable);
//            nodeTable.AddLink(nodeTableName);
//            nodeTableName.AddLink(nodeLeftParen);

//            createTableBlock.Capture(nodeTable, nodeTableName, nodeLeftParen);

//            // <column_definition>
//            var columnName = new IdentifierNode(
//                (token, context) =>
//                    context.GetLastResult<dynamic>().Columns.Add(new DynamicResult(new { Name = ((WordToken)token).Word })),
//                "Node: column name");

//            var columnType = new IdentifierNode(
//                (token, context) =>
//                {
//                    var table = context.GetLastResult<dynamic>();
//                    var columns = table.Columns;
//                    var columnCount = columns.Count;
//                    var column = columns[columnCount - 1];
//                    var type = ((WordToken)token).Word;
//                    column.Type = type;
//                },
//                "Node: column type");
//            columnName.AddLink(columnType);

//            var columnDefinition = new Block(columnName, "Block: column definition");
//            columnDefinition.Capture(columnType);

//            nodeLeftParen.AddLink(columnDefinition);

//            // ',' and ')'
//            var columnComma = new ExactSymbolNode(',', ParsingHelper.IdleTokenProcessor, "Node: ,");
//            columnComma.AddLink(columnDefinition);


//            columnType.AddLink(columnComma);

//            var rightParen = new ExactSymbolNode(')', ParsingHelper.IdleTokenProcessor, "Node: )");
//            columnType.AddLink(rightParen);

//            // end
//            rightParen.AddLink(EndNode.Instance);

//            // super-block.
//            var superBlock = new Block(createTableBlock, "Block: super block");
//            superBlock.Capture(columnDefinition, columnComma, rightParen);

//            superBlock.FinalizeUnit();

//            return superBlock;
//        }

//        public IContext Parse(IEnumerable<IToken> tokens)
//        {
//            var context = new Context();
//            var stream = new TokenStream(tokens);
//            var current = _head;

//            do
//            {
//                var result = current.Process(stream, context);
//                if (result == null)
//                {
//                    throw new Exception();
//                }
//                else if (result.Count == 1)
//                {
//                    if (ParsingHelper.IsEndResult(result))
//                    {
//                        return context;
//                    }
//                    else
//                    {
//                        throw new Exception();
//                    }
//                }
//                else
//                {
//                    throw new Exception();
//                }

//            } while (true);
//        }
//    }
//}
