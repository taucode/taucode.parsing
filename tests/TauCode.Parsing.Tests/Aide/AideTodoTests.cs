using NUnit.Framework;

namespace TauCode.Parsing.Tests.Aide
{
    [TestFixture]
    public class AideTodoTests
    {
        [Test]
        public void TodoFoo()
        {
            // Arrange
            var createTableBlockText =
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

<begin>CONSTRAINT { <primary_key>\Block | <foreign_key>\Block } \Link(:create_table, :table_closing) [<comma>\,] \Link(:begin) \WrongWay

\EndBlock


/****** Primary Key ******/

\BeginBlock(:primary_key)

PRIMARY KEY <pk_name>\Identifier \( <pk_columns>\Block \)

\EndBlock

\BeginBlock(pk_columns)

<pk_column_name>\Identifier [{<asc>ASC | <desc>DESC}]

\EndBlock

\BeginBlock(foreign_key)

FOREIGN KEY <fk_name>\Identifier <fk_columns>\Block REFERENCES <fk_referenced_columns>\Block

\EndBlock

\BeginBlock(fk_columns)

\( <fk_column_name>\Identifier [<comma>\,] \)

\BeginLinks
comma -> fk_column_name
\EndLinks
\EndBlock

\CloneBlock(fk_referenced_columns, fk_columns)
";

            // Act

            // Assert
        }
    }
}
