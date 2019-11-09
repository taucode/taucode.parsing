using NUnit.Framework;
using System;
using TauCode.Parsing.Aide;

namespace TauCode.Parsing.Tests.Aide
{
    [TestFixture]
    public class AideParserTests
    {
        [Test]
        public void TodoFoo()
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

<column_name>\Identifier <type_name>\Identifier \Link(:idle) <optional_nullability>[ NULL /*{ <null>NULL | NOT <not_null>NULL }*/] <idle>\Idle


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
            throw new NotImplementedException();
        }
    }
}
