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
\BeginBlock(create_table)

CREATE TABLE <table_name>\Identifier \(
    <column_definition>\Block <comma>\,
    <constraint_definitions>\Block
<table_closing>\)

\BeginLinks
comma -> column_definition
column_definition -> table_closing
\EndLinks
\EndBlock

\BeginBlock(column_definition)

<column_name>\Identifier <type_name>\Identifier [{ <null>NULL | NOT <not_null>NULL }] \Idle

\EndBlock

\BeginBlock(constraint_definitions)

CONSTRAINT { <primary_key>\Block | <foreign_key>\Block } <comma>\,

\EndBlock

\BeginBlock(primary_key)

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
