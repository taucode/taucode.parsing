using NUnit.Framework;
using System;
using System.Linq;
using TauCode.Parsing.Aide;
using TauCode.Parsing.Aide.Results;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.Aide
{
    [TestFixture]
    public class AideParserTests
    {
        [Test]
        public void Parse_ValidInput_Parses()
        {
            // Arrange
            var input =
@"
/****** Create table ******/

\BeginBlockDefinition(:create_table, :top)

CREATE TABLE <table_name>\Identifier \(
    <column_definition>\BlockReference \Link(:table_closing) <comma>\, \Link(:column_definition)
    <constraint_definitions>\BlockReference
<table_closing>\)

\EndBlockDefinition


/****** Column Definition ******/

\BeginBlockDefinition(:column_definition)

<column_name>\Identifier <type_name>\Identifier \Link(:idle) <optional_nullability>[{ <null>NULL | NOT <not_null>NULL }]

\EndBlockDefinition

/****** Constraint Definitions ******/

\BeginBlockDefinition(:constraint_definitions)

<begin>CONSTRAINT { <primary_key>\BlockReference | <foreign_key>\BlockReference } { \Link(:create_table, :table_closing) | <comma>\, \Link(:begin) }

\EndBlockDefinition


/****** Primary Key ******/

\BeginBlockDefinition(:primary_key)

PRIMARY KEY <pk_name>\Identifier <pk_columns>\BlockReference

\EndBlockDefinition


/****** Primary Key Columns ******/

\BeginBlockDefinition(:pk_columns)

\( <pk_column_name>\Identifier [ { <asc>ASC | <desc>DESC } ] { \, \Link(:pk_column_name) | \) }

\EndBlockDefinition


/****** Foreign Key ******/

\BeginBlockDefinition(:foreign_key)

FOREIGN KEY <fk_name>\Identifier <fk_columns>\BlockReference REFERENCES <fk_referenced_columns>\BlockReference

\EndBlockDefinition


/****** Foreign Key Columns ******/

\BeginBlockDefinition(:fk_columns)

\( <fk_column_name>\Identifier { \, \Link(:fk_column_name) | \) }

\EndBlockDefinition


/****** Foreign Key Referenced Columns ******/

\CloneBlock(:fk_referenced_columns, :fk_columns)
";

            var lexer = new AideLexer();
            var tokens = lexer.Lexize(input);

            // Act
            IParser parser = new AideParser();
            var context = parser.Parse(tokens);

            // Assert
            var results = context.ToArray();

            #region *** Block Definition: Create Table ***

            var result = results[0];
            Assert.That(result, Is.TypeOf<BlockDefinitionResult>());
            var blockDefinitionResult = (BlockDefinitionResult)result;
            var names = blockDefinitionResult.Arguments.ToList();
            var content = blockDefinitionResult.Content;

            Assert.That(names, Has.Count.EqualTo(2));
            Assert.That(names[0], Is.EqualTo("create_table"));
            Assert.That(names[1], Is.EqualTo("top"));

            var unitResults = content.GetAllResults();

            Assert.That(unitResults, Has.Count.EqualTo(10));

            // CREATE
            var unitResult = unitResults[0];
            var unitResultFormatting = unitResult.FormatUnitResult();
            Assert.That(unitResultFormatting, Is.EqualTo(@"CREATE"));

            // TABLE
            unitResult = unitResults[1];
            unitResultFormatting = unitResult.FormatUnitResult();
            Assert.That(unitResultFormatting, Is.EqualTo(@"TABLE"));

            // <table_name>\Identifier
            unitResult = unitResults[2];
            unitResultFormatting = unitResult.FormatUnitResult();
            Assert.That(unitResultFormatting, Is.EqualTo(@"<table_name>\Identifier"));

            // \(
            unitResult = unitResults[3];
            Assert.That(unitResult, Is.TypeOf<SymbolNodeResult>());
            var symbolNodeResult = (SymbolNodeResult)unitResult;
            Assert.That(symbolNodeResult.Value, Is.EqualTo(SymbolValue.LeftParenthesis));
            Assert.That(symbolNodeResult.SourceNodeName, Is.Null);

            // <column_definition>\BlockReference
            unitResult = unitResults[4];
            Assert.That(unitResult, Is.TypeOf<BlockReferenceResult>());
            var blockReferenceResult = (BlockReferenceResult)unitResult;
            Assert.That(blockReferenceResult.SourceNodeName, Is.EqualTo("column_definition"));

            // \Link(:table_closing)
            unitResult = unitResults[5];
            Assert.That(unitResult, Is.TypeOf<LinkResult>());
            var linkResult = (LinkResult)unitResult;
            var linkArguments = linkResult.Arguments.ToList();
            Assert.That(linkArguments, Has.Count.EqualTo(1));
            var linkArgument = linkArguments[0];
            Assert.That(linkArgument, Is.EqualTo("table_closing"));
            Assert.That(linkResult.SourceNodeName, Is.Null);

            // <comma>\,
            unitResult = unitResults[6];
            Assert.That(unitResult, Is.TypeOf<SymbolNodeResult>());
            symbolNodeResult = (SymbolNodeResult)unitResult;
            Assert.That(symbolNodeResult.Value, Is.EqualTo(SymbolValue.Comma));
            Assert.That(symbolNodeResult.SourceNodeName, Is.EqualTo("comma"));

            // \Link(:column_definition)
            unitResult = unitResults[7];
            Assert.That(unitResult, Is.TypeOf<LinkResult>());
            linkResult = (LinkResult)unitResult;
            linkArguments = linkResult.Arguments.ToList();
            Assert.That(linkArguments, Has.Count.EqualTo(1));
            linkArgument = linkArguments[0];
            Assert.That(linkArgument, Is.EqualTo("column_definition"));
            Assert.That(linkResult.SourceNodeName, Is.Null);

            // <constraint_definitions>\BlockReference
            unitResult = unitResults[8];
            Assert.That(unitResult, Is.TypeOf<BlockReferenceResult>());
            blockReferenceResult = (BlockReferenceResult)unitResult;
            Assert.That(blockReferenceResult.SourceNodeName, Is.EqualTo("constraint_definitions"));

            // <table_closing>\)
            unitResult = unitResults[9];
            Assert.That(unitResult, Is.TypeOf<SymbolNodeResult>());
            symbolNodeResult = (SymbolNodeResult)unitResult;
            Assert.That(symbolNodeResult.Value, Is.EqualTo(SymbolValue.RightParenthesis));
            Assert.That(symbolNodeResult.SourceNodeName, Is.EqualTo("table_closing"));

            #endregion

            #region *** Column Definition ***

            result = results[1];
            Assert.That(result, Is.TypeOf<BlockDefinitionResult>());
            blockDefinitionResult = (BlockDefinitionResult)result;
            names = blockDefinitionResult.Arguments.ToList();
            content = blockDefinitionResult.Content;

            Assert.That(names, Has.Count.EqualTo(1));
            Assert.That(names[0], Is.EqualTo("column_definition"));

            unitResults = content.GetAllResults();

            Assert.That(unitResults, Has.Count.EqualTo(4));

            // <column_name>\Identifier
            unitResult = unitResults[0];
            unitResultFormatting = unitResult.FormatUnitResult();
            Assert.That(unitResultFormatting, Is.EqualTo(@"<column_name>\Identifier"));

            //Assert.That(unitResult, Is.TypeOf<IdentifierNodeResult>());
            //identifierNodeResult = (IdentifierNodeResult)unitResult;
            //Assert.That(identifierNodeResult.SourceNodeName, Is.EqualTo("column_name"));

            // <type_name>\Identifier
            unitResult = unitResults[1];
            Assert.That(unitResult, Is.TypeOf<IdentifierNodeResult>());
            var identifierNodeResult = (IdentifierNodeResult)unitResult;
            Assert.That(identifierNodeResult.SourceNodeName, Is.EqualTo("type_name"));

            // \Link(:idle)
            unitResult = unitResults[2];
            Assert.That(unitResult, Is.TypeOf<LinkResult>());
            linkResult = (LinkResult)unitResult;
            linkArguments = linkResult.Arguments.ToList();
            Assert.That(linkArguments, Has.Count.EqualTo(1));
            linkArgument = linkArguments[0];
            Assert.That(linkArgument, Is.EqualTo("idle"));
            Assert.That(linkResult.SourceNodeName, Is.Null);

            // <optional_nullability>[{ <null>NULL | NOT <not_null>NULL }]
            unitResult = unitResults[3];
            Assert.That(unitResult, Is.TypeOf<OptionalResult>());
            var optionalResult = (OptionalResult)unitResult;
            Assert.That(optionalResult.SourceNodeName, Is.EqualTo("optional_nullability"));

            var content2 = optionalResult.OptionalContent;
            var unitResults2 = content2.GetAllResults();
            var unitResult2 = unitResults2.Single();

            Assert.That(unitResult2, Is.TypeOf<AlternativesResult>());
            var alternativesResult = (AlternativesResult)unitResult2;

            var alternatives = alternativesResult.GetAllAlternatives();
            Assert.That(alternatives, Has.Count.EqualTo(2));

            var content3 = alternatives[0];
            unitResult = content3.GetAllResults().Single();
            Assert.That(unitResult, Is.TypeOf<WordNodeResult>());
            var wordNodeResult = (WordNodeResult)unitResult;
            Assert.That(wordNodeResult.Word, Is.EqualTo("NULL"));
            Assert.That(wordNodeResult.SourceNodeName, Is.EqualTo("null"));

            content3 = alternatives[1];
            unitResults = content3.GetAllResults();
            Assert.That(unitResults, Has.Count.EqualTo(2));

            unitResult = unitResults[0];
            Assert.That(unitResult, Is.TypeOf<WordNodeResult>());
            wordNodeResult = (WordNodeResult)unitResult;
            Assert.That(wordNodeResult.Word, Is.EqualTo("NOT"));
            Assert.That(wordNodeResult.SourceNodeName, Is.Null);

            unitResult = unitResults[1];
            Assert.That(unitResult, Is.TypeOf<WordNodeResult>());
            wordNodeResult = (WordNodeResult)unitResult;
            Assert.That(wordNodeResult.Word, Is.EqualTo("NULL"));
            Assert.That(wordNodeResult.SourceNodeName, Is.EqualTo("not_null"));

            #endregion








            throw new NotImplementedException("go on!");

        }
    }
}
