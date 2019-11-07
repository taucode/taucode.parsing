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

\BeginBlock(:create_table)

CREATE TABLE <table_name>\Identifier \(
    <column_definition>\Block \Link(:table_closing) <comma>\, \Link(:column_definition)
    <constraint_definitions>\Block
<table_closing>\)

\EndBlock


/****** Column Definition ******/

\BeginBlock(:column_definition)

<column_name>\Identifier <type_name>\Identifier \Link(:idle) [{ <null>NULL | NOT <not_null>NULL }] <idle>\Idle


/****** Constraint Definitions ******/

\BeginBlock(:constraint_definitions)

<begin>CONSTRAINT { <primary_key>\Block | <foreign_key>\Block } { \Link(:create_table, :table_closing) | <comma>\, \Link(:begin) }

\EndBlock


/****** Primary Key ******/

\BeginBlock(:primary_key)

PRIMARY KEY <pk_name>\Identifier <pk_columns>\Block

\EndBlock


/****** Primary Key Columns ******/

\BeginBlock(:pk_columns)

\( <pk_column_name>\Identifier [ { <asc>ASC | <desc>DESC } ] { \, \Link(:pk_column_name) | \) }

\EndBlock


/****** Foreign Key ******/

\BeginBlock(:foreign_key)

FOREIGN KEY <fk_name>\Identifier <fk_columns>\Block REFERENCES <fk_referenced_columns>\Block

\EndBlock


/****** Foreign Key Columns ******/

\BeginBlock(:fk_columns)

\( <fk_column_name>\Identifier { \, \Link(:fk_column_name) | \) }

\EndBlock


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
