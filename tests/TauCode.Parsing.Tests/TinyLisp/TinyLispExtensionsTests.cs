using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Data;
using TauCode.Utils.Extensions;

namespace TauCode.Parsing.Tests.TinyLisp
{
    [TestFixture]
    public class TinyLispExtensionsTests
    {
        private PseudoList _inputTokens;

        [SetUp]
        public void SetUp()
        {
            var input = this.GetType().Assembly.GetResourceText("sql-grammar.lisp", true);
            ILexer lexer = new TinyLispLexer();

            var tokens = lexer.Lexize(input);

            var reader = new TinyLispPseudoReader();
            _inputTokens = reader.Read(tokens);
        }

        [Test]
        public void TinyLispExtensions_Create_ProducesValidResults()
        {
            // Arrange

            var defBlock = _inputTokens[0].AsPseudoList();

            // Act
            var verb = defBlock.GetCarSymbolName();
            var name = defBlock.GetSingleKeywordArgument<Symbol>(":name").Name;
            var isTop = defBlock.GetSingleArgumentAsBool(":is-top");

            var free = defBlock.GetFreeArguments();

            var word = free.First().AsPseudoList();
            var alt = free.Last().AsPseudoList();

            var altArgs = alt.GetFreeArguments();

            var altBlock1 = altArgs.First().AsPseudoList();
            var altBlock2 = altArgs.Last().AsPseudoList();

            // Assert
            Assert.That(verb, Is.EqualTo("defblock").IgnoreCase);
            Assert.That(name, Is.EqualTo("create").IgnoreCase);
            Assert.That(isTop, Is.True);

            Assert.That(word.GetCarSymbolName(), Is.EqualTo("word").IgnoreCase);
            Assert.That(word.GetSingleKeywordArgument<StringAtom>(":value").Value, Is.EqualTo("CREATE").IgnoreCase);

            Assert.That(altBlock1.GetCarSymbolName(), Is.EqualTo("block").IgnoreCase);
            Assert.That(altBlock1.GetSingleKeywordArgument<Symbol>(":ref").Name, Is.EqualTo("create-table").IgnoreCase);

            Assert.That(altBlock2.GetCarSymbolName(), Is.EqualTo("block").IgnoreCase);
            Assert.That(altBlock2.GetSingleKeywordArgument<Symbol>(":ref").Name, Is.EqualTo("create-index").IgnoreCase);
        }

        [Test]
        public void TinyLispExtensions_CreateTable_ProducesValidResults()
        {
            // Arrange

            var defBlock = _inputTokens[1].AsPseudoList();

            // Act
            var verb = defBlock.GetCarSymbolName();
            var name = defBlock.GetSingleKeywordArgument<Symbol>(":name").Name;

            var free = defBlock.GetFreeArguments();

            // Assert
            Assert.That(verb, Is.EqualTo("defblock").IgnoreCase);
            Assert.That(name, Is.EqualTo("create-table").IgnoreCase);

            Assert.That(free, Has.Count.EqualTo(7));

            var word = free.Single(x => x.GetCarSymbolName() == "WORD");
            var alt = free[1].AsPseudoList();
            var symbol0 = free.Where(x => x.GetCarSymbolName() == "SYMBOL").ElementAt(0);
            var block0 = free.Where(x => x.GetCarSymbolName() == "BLOCK").ElementAt(0);
            var symbol1 = free.Where(x => x.GetCarSymbolName() == "SYMBOL").ElementAt(1);
            var block1 = free.Where(x => x.GetCarSymbolName() == "BLOCK").ElementAt(1);
            var symbol2 = free.Where(x => x.GetCarSymbolName() == "SYMBOL").ElementAt(2);

            // word
            Assert.That(word.GetSingleKeywordArgument<StringAtom>(":value").Value, Is.EqualTo("TABLE"));

            // alt
            Assert.That(alt.GetCarSymbolName(), Is.EqualTo("alt").IgnoreCase);
            var altArgs = alt.GetFreeArguments();
            Assert.That(altArgs[0].GetCarSymbolName(), Is.EqualTo("some-ident").IgnoreCase);
            Assert.That(altArgs[0].GetSingleKeywordArgument<Symbol>(":name").Name,
                Is.EqualTo("table-name-ident").IgnoreCase);
            Assert.That(altArgs[1].GetCarSymbolName(), Is.EqualTo("some-word").IgnoreCase);
            Assert.That(altArgs[1].GetSingleKeywordArgument<Symbol>(":name").Name,
                Is.EqualTo("table-name-word").IgnoreCase);

            // symbol 0
            Assert.That(symbol0.GetSingleKeywordArgument<StringAtom>(":value").Value, Is.EqualTo("("));

            // block 0
            Assert.That(block0.GetSingleKeywordArgument<Symbol>(":ref").Name, Is.EqualTo("column-def").IgnoreCase);
            CollectionAssert.AreEqual(
                new List<Symbol>
                {
                    Symbol.Create("table-closing"),
                    Symbol.Create("next"),
                },
                block0.GetAllKeywordArguments(":links"));

            // symbol 1
            Assert.That(symbol1.GetSingleKeywordArgument<StringAtom>(":value").Value, Is.EqualTo(","));
            CollectionAssert.AreEqual(
                new List<Symbol>
                {
                    Symbol.Create("column-def"),
                    Symbol.Create("next"),
                },
                symbol1.GetAllKeywordArguments(":links"));

            // block 1
            Assert.That(block1.GetSingleKeywordArgument<Symbol>(":ref").Name, Is.EqualTo("constraint-defs").IgnoreCase);

            // symbol 2
            Assert.That(symbol2.GetSingleKeywordArgument<StringAtom>(":value").Value, Is.EqualTo(")"));
            Assert.That(symbol2.GetSingleKeywordArgument<Symbol>(":name").Name, Is.EqualTo("table-closing").IgnoreCase);
        }

        [Test]
        public void TinyLispExtensions_ColumnDefinition_ProducesValidResults()
        {
            // Arrange

            var defBlock = _inputTokens[2];

            // Act
            var verb = defBlock.GetCarSymbolName();
            var name = defBlock.GetSingleKeywordArgument<Symbol>(":name").Name;

            var free = defBlock.GetFreeArguments();

            // Assert
            Assert.That(verb, Is.EqualTo("defblock").IgnoreCase);
            Assert.That(name, Is.EqualTo("column-def").IgnoreCase);

            Assert.That(free, Has.Count.EqualTo(6));

            var alt0 = free[0];
            var alt1 = free[1];
            var opt0 = free[2];
            var opt1 = free[3];
            var opt2 = free[4];
            var opt3 = free[5];

            // alt0
            Assert.That(alt0.GetCarSymbolName(), Is.EqualTo("alt").IgnoreCase);
            free = alt0.GetFreeArguments();
            Assert.That(free[0].GetCarSymbolName(), Is.EqualTo("some-ident").IgnoreCase);
            Assert.That(free[0].GetSingleKeywordArgument<Symbol>(":name").Name, Is.EqualTo("column-name-ident").IgnoreCase);
            Assert.That(free[1].GetCarSymbolName(), Is.EqualTo("some-word").IgnoreCase);
            Assert.That(free[1].GetSingleKeywordArgument<Symbol>(":name").Name, Is.EqualTo("column-name-word").IgnoreCase);

            // alt1
            Assert.That(alt1.GetCarSymbolName(), Is.EqualTo("alt").IgnoreCase);
            free = alt1.GetFreeArguments();
            Assert.That(free[0].GetCarSymbolName(), Is.EqualTo("some-ident").IgnoreCase);
            Assert.That(free[0].GetSingleKeywordArgument<Symbol>(":name").Name, Is.EqualTo("type-name-ident").IgnoreCase);
            Assert.That(free[1].GetCarSymbolName(), Is.EqualTo("some-word").IgnoreCase);
            Assert.That(free[1].GetSingleKeywordArgument<Symbol>(":name").Name, Is.EqualTo("type-name-word").IgnoreCase);

            // opt0
            free = opt0.GetFreeArguments();
            Assert.That(free, Has.Count.EqualTo(4));
            Assert.That(free[0].GetCarSymbolName(), Is.EqualTo("symbol").IgnoreCase);
            Assert.That(free[0].GetSingleKeywordArgument<StringAtom>(":value").Value, Is.EqualTo("(").IgnoreCase);
            Assert.That(free[1].GetCarSymbolName(), Is.EqualTo("some-int").IgnoreCase);
            Assert.That(free[1].GetSingleKeywordArgument<Symbol>(":name").Name, Is.EqualTo("precision").IgnoreCase);
            var innerOpt = free[2];
            var innerOptFree = innerOpt.GetFreeArguments();
            Assert.That(innerOptFree, Has.Count.EqualTo(2));
            Assert.That(innerOptFree[0].GetCarSymbolName(), Is.EqualTo("symbol").IgnoreCase);
            Assert.That(innerOptFree[0].GetSingleKeywordArgument<StringAtom>(":value").Value, Is.EqualTo(",").IgnoreCase);
            Assert.That(innerOptFree[1].GetCarSymbolName(), Is.EqualTo("some-int").IgnoreCase);
            Assert.That(innerOptFree[1].GetSingleKeywordArgument<Symbol>(":name").Name, Is.EqualTo("scale").IgnoreCase);
            Assert.That(free[3].GetCarSymbolName(), Is.EqualTo("symbol").IgnoreCase);
            Assert.That(free[3].GetSingleKeywordArgument<StringAtom>(":value").Value, Is.EqualTo(")").IgnoreCase);

            // opt1
            Assert.That(opt1.GetCarSymbolName(), Is.EqualTo("opt").IgnoreCase);
            free = opt1.GetFreeArguments();
            var innerAlt = free.Single();
            var innerAltFree = innerAlt.GetFreeArguments();
            Assert.That(innerAltFree, Has.Count.EqualTo(2));
            Assert.That(innerAlt.GetCarSymbolName(), Is.EqualTo("alt").IgnoreCase);
            Assert.That(innerAltFree[0].GetCarSymbolName(), Is.EqualTo("word").IgnoreCase);
            Assert.That(innerAltFree[0].GetSingleKeywordArgument<StringAtom>(":value").Value, Is.EqualTo("NULL"));
            Assert.That(innerAltFree[0].GetSingleKeywordArgument<Symbol>(":name").Name, Is.EqualTo("null").IgnoreCase);
            var seq = innerAltFree[1];
            var seqFree = seq.GetFreeArguments();
            Assert.That(seqFree, Has.Count.EqualTo(2));
            Assert.That(seqFree[0].GetSingleKeywordArgument<StringAtom>(":value").Value, Is.EqualTo("NOT"));
            Assert.That(seqFree[1].GetSingleKeywordArgument<StringAtom>(":value").Value, Is.EqualTo("NULL"));
            Assert.That(seqFree[1].GetSingleKeywordArgument<Symbol>(":name").Name, Is.EqualTo("not-null").IgnoreCase);

            // opt2
            Assert.That(opt2.GetCarSymbolName(), Is.EqualTo("opt").IgnoreCase);
            free = opt2.GetFreeArguments();
            Assert.That(free, Has.Count.EqualTo(2));
            Assert.That(free[0].GetCarSymbolName(), Is.EqualTo("word").IgnoreCase);
            Assert.That(free[0].GetSingleKeywordArgument<StringAtom>(":value").Value, Is.EqualTo("PRIMARY"));
            Assert.That(free[1].GetCarSymbolName(), Is.EqualTo("word").IgnoreCase);
            Assert.That(free[1].GetSingleKeywordArgument<StringAtom>(":value").Value, Is.EqualTo("KEY"));
            Assert.That(free[1].GetSingleKeywordArgument<Symbol>(":name").Name, Is.EqualTo("inline-primary-key").IgnoreCase);

            // opt3
            Assert.That(opt3.GetCarSymbolName(), Is.EqualTo("opt").IgnoreCase);
            free = opt3.GetFreeArguments();
            Assert.That(free, Has.Count.EqualTo(2));
            Assert.That(free[0].GetCarSymbolName(), Is.EqualTo("word").IgnoreCase);
            Assert.That(free[0].GetSingleKeywordArgument<StringAtom>(":value").Value, Is.EqualTo("DEFAULT"));
            innerAlt = free[1];
            innerAltFree = innerAlt.GetFreeArguments();
            Assert.That(innerAltFree, Has.Count.EqualTo(3));
            Assert.That(innerAlt.GetCarSymbolName(), Is.EqualTo("alt").IgnoreCase);
            Assert.That(innerAltFree[0].GetCarSymbolName(), Is.EqualTo("word").IgnoreCase);
            Assert.That(innerAltFree[0].GetSingleKeywordArgument<StringAtom>(":value").Value, Is.EqualTo("NULL"));
            Assert.That(innerAltFree[0].GetSingleKeywordArgument<Symbol>(":name").Name, Is.EqualTo("default-null").IgnoreCase);
            Assert.That(innerAltFree[1].GetCarSymbolName(), Is.EqualTo("some-int").IgnoreCase);
            Assert.That(innerAltFree[1].GetSingleKeywordArgument<Symbol>(":name").Name, Is.EqualTo("default-integer").IgnoreCase);
            Assert.That(innerAltFree[2].GetCarSymbolName(), Is.EqualTo("some-string").IgnoreCase);
            Assert.That(innerAltFree[2].GetSingleKeywordArgument<Symbol>(":name").Name, Is.EqualTo("default-string").IgnoreCase);
        }

        [Test]
        public void TinyLispExtensions_ConstraintDefinitions_ProducesValidResults()
        {
            // Arrange
            var defBlock = _inputTokens[3];

            // Act
            var verb = defBlock.GetCarSymbolName();
            var name = defBlock.GetSingleKeywordArgument<Symbol>(":name").Name;

            var free = defBlock.GetFreeArguments();

            // Assert
            Assert.That(verb, Is.EqualTo("defblock").IgnoreCase);
            Assert.That(name, Is.EqualTo("constraint-definitions").IgnoreCase);

            Assert.That(free, Has.Count.EqualTo(5));

            var word = free[0];
            var alt0 = free[1];
            var alt1 = free[2];
            var symbol = free[3];
            var idle = free[4];

            // word
            Assert.That(word.GetCarSymbolName(), Is.EqualTo("word").IgnoreCase);
            Assert.That(word.GetSingleKeywordArgument<StringAtom>(":value").Value, Is.EqualTo("CONSTRAINT"));
            Assert.That(word.GetSingleKeywordArgument<Symbol>(":name").Name, Is.EqualTo("constraint").IgnoreCase);

            // alt0
            var altFree = alt0.GetFreeArguments();
            Assert.That(altFree[0].GetCarSymbolName(), Is.EqualTo("some-ident").IgnoreCase);
            Assert.That(altFree[0].GetSingleKeywordArgument<Symbol>(":name").Name, Is.EqualTo("constraint-name-ident").IgnoreCase);
            Assert.That(altFree[1].GetCarSymbolName(), Is.EqualTo("some-word").IgnoreCase);
            Assert.That(altFree[1].GetSingleKeywordArgument<Symbol>(":name").Name, Is.EqualTo("constraint-name-word").IgnoreCase);

            // alt1
            altFree = alt1.GetFreeArguments();
            Assert.That(altFree[0].GetCarSymbolName(), Is.EqualTo("block").IgnoreCase);
            Assert.That(altFree[0].GetSingleKeywordArgument<Symbol>(":ref").Name, Is.EqualTo("primary-key").IgnoreCase);
            Assert.That(altFree[1].GetCarSymbolName(), Is.EqualTo("block").IgnoreCase);
            Assert.That(altFree[1].GetSingleKeywordArgument<Symbol>(":ref").Name, Is.EqualTo("foreign-key").IgnoreCase);

            // symbol
            Assert.That(symbol.GetCarSymbolName(), Is.EqualTo("symbol").IgnoreCase);
            Assert.That(symbol.GetSingleKeywordArgument<StringAtom>(":value").Value, Is.EqualTo(","));
            free = symbol.GetAllKeywordArguments(":links");
            CollectionAssert.AreEqual(
                new List<Symbol>
                {
                    Symbol.Create("constraint")
                },
                free);

            // idle
            Assert.That(idle.ToString(), Is.EqualTo("(IDLE)"));

        }

        [Test]
        public void TinyLispExtensions_PrimaryKey_ProducesValidResults()
        {
            // Arrange
            var defBlock = _inputTokens[4];

            // Act
            var verb = defBlock.GetCarSymbolName();
            var name = defBlock.GetSingleKeywordArgument<Symbol>(":name").Name;

            var free = defBlock.GetFreeArguments();

            // Assert
            Assert.That(verb, Is.EqualTo("defblock").IgnoreCase);
            Assert.That(name, Is.EqualTo("primary-key").IgnoreCase);

            Assert.That(free, Has.Count.EqualTo(3));

            var word0 = free[0];
            var word1 = free[1];
            var block = free[2];

            // word 0
            Assert.That(word0.GetCarSymbolName(), Is.EqualTo("word").IgnoreCase);
            Assert.That(word0.GetSingleKeywordArgument<StringAtom>(":value").Value, Is.EqualTo("PRIMARY"));
            Assert.That(word0.GetSingleKeywordArgument<Symbol>(":name").Name, Is.EqualTo("do-primary-key").IgnoreCase);

            // word 1
            Assert.That(word1.GetCarSymbolName(), Is.EqualTo("word").IgnoreCase);
            Assert.That(word1.GetSingleKeywordArgument<StringAtom>(":value").Value, Is.EqualTo("KEY"));

            // block
            Assert.That(block.GetCarSymbolName(), Is.EqualTo("block").IgnoreCase);
            Assert.That(block.GetSingleKeywordArgument<Symbol>(":ref").Name, Is.EqualTo("pk-columns").IgnoreCase);
        }

        [Test]
        public void TinyLispExtensions_PrimaryKeyColumns_ProducesValidResults()
        {
            // Arrange
            var defBlock = _inputTokens[5];

            // Act
            var verb = defBlock.GetCarSymbolName();
            var name = defBlock.GetSingleKeywordArgument<Symbol>(":name").Name;

            var free = defBlock.GetFreeArguments();

            // Assert
            Assert.That(verb, Is.EqualTo("defblock").IgnoreCase);
            Assert.That(name, Is.EqualTo("pk-columns").IgnoreCase);

            Assert.That(free, Has.Count.EqualTo(5));

            var symbol0 = free[0];
            var alt0 = free[1];
            var opt = free[2];
            var alt1 = free[3];
            var symbol1 = free[4];

            // symbol 0
            Assert.That(symbol0.GetCarSymbolName(), Is.EqualTo("symbol").IgnoreCase);
            Assert.That(symbol0.GetSingleKeywordArgument<StringAtom>(":value").Value, Is.EqualTo("("));

            // alt 0
            Assert.That(alt0.GetCarSymbolName(), Is.EqualTo("alt").IgnoreCase);
            Assert.That(alt0.GetSingleKeywordArgument<Symbol>(":name").Name, Is.EqualTo("pk-column-name-alternatives").IgnoreCase);
            free = alt0.GetFreeArguments();
            Assert.That(free, Has.Count.EqualTo(2));
            Assert.That(free[0].GetCarSymbolName(), Is.EqualTo("some-ident").IgnoreCase);
            Assert.That(free[0].GetSingleKeywordArgument<Symbol>(":name").Name, Is.EqualTo("pk-column-name-ident").IgnoreCase);
            Assert.That(free[1].GetCarSymbolName(), Is.EqualTo("some-word").IgnoreCase);
            Assert.That(free[1].GetSingleKeywordArgument<Symbol>(":name").Name, Is.EqualTo("pk-column-name-word").IgnoreCase);

            // opt
            Assert.That(opt.GetCarSymbolName(), Is.EqualTo("opt").IgnoreCase);
            free = opt.GetFreeArguments();
            Assert.That(free[0].GetCarSymbolName(), Is.EqualTo("word").IgnoreCase);
            Assert.That(free[0].GetSingleKeywordArgument<StringAtom>(":value").Value, Is.EqualTo("ASC"));
            Assert.That(free[0].GetSingleKeywordArgument<Symbol>(":name").Name, Is.EqualTo("asc").IgnoreCase);
            Assert.That(free[1].GetSingleKeywordArgument<StringAtom>(":value").Value, Is.EqualTo("DESC"));
            Assert.That(free[1].GetCarSymbolName(), Is.EqualTo("word").IgnoreCase);
            Assert.That(free[1].GetSingleKeywordArgument<Symbol>(":name").Name, Is.EqualTo("desc").IgnoreCase);

            // alt 1
            Assert.That(alt1.GetCarSymbolName(), Is.EqualTo("alt").IgnoreCase);
            free = alt1.GetFreeArguments();
            Assert.That(free, Has.Count.EqualTo(2));
            Assert.That(free[0].GetCarSymbolName(), Is.EqualTo("symbol").IgnoreCase);
            Assert.That(free[0].GetSingleKeywordArgument<StringAtom>(":value").Value, Is.EqualTo(","));
            var links = free[0].GetAllKeywordArguments(":links");
            CollectionAssert.AreEqual(
                new List<Symbol>
                {
                    Symbol.Create("pk-column-name-alternatives"),
                },
                links);
            Assert.That(free[1].ToString(), Is.EqualTo("(idle)").IgnoreCase);

            // symbol 1
            Assert.That(symbol1.GetCarSymbolName(), Is.EqualTo("symbol").IgnoreCase);
            Assert.That(symbol1.GetSingleKeywordArgument<StringAtom>(":value").Value, Is.EqualTo(")"));
        }

        [Test]
        public void TinyLispExtensions_ForeignKey_ProducesValidResults()
        {
            // Arrange
            var defBlock = _inputTokens[6];

            // Act
            var verb = defBlock.GetCarSymbolName();
            var name = defBlock.GetSingleKeywordArgument<Symbol>(":name").Name;

            var free = defBlock.GetFreeArguments();

            // Assert
            Assert.That(verb, Is.EqualTo("defblock").IgnoreCase);
            Assert.That(name, Is.EqualTo("foreign-key").IgnoreCase);

            Assert.That(free, Has.Count.EqualTo(6));

            var word0 = free[0];
            var word1 = free[1];
            var block0 = free[2];
            var word2 = free[3];
            var alt = free[4];
            var block1 = free[5];

            // word 0
            Assert.That(word0.GetCarSymbolName(), Is.EqualTo("word").IgnoreCase);
            Assert.That(word0.GetSingleKeywordArgument<StringAtom>(":value").Value, Is.EqualTo("FOREIGN"));
            Assert.That(word0.GetSingleKeywordArgument<Symbol>(":name").Name, Is.EqualTo("do-primary-key").IgnoreCase);

            // word 1
            Assert.That(word1.GetCarSymbolName(), Is.EqualTo("word").IgnoreCase);
            Assert.That(word1.GetSingleKeywordArgument<StringAtom>(":value").Value, Is.EqualTo("KEY"));

            // block 0
            Assert.That(block0.GetCarSymbolName(), Is.EqualTo("block").IgnoreCase);
            Assert.That(block0.GetSingleKeywordArgument<Symbol>(":ref").Name, Is.EqualTo("fk-columns").IgnoreCase);

            // word 2
            Assert.That(word2.GetCarSymbolName(), Is.EqualTo("word").IgnoreCase);
            Assert.That(word2.GetSingleKeywordArgument<StringAtom>(":value").Value, Is.EqualTo("REFERENCES"));

            // alt
            Assert.That(alt.GetCarSymbolName(), Is.EqualTo("alt").IgnoreCase);
            free = alt.GetFreeArguments();
            Assert.That(free, Has.Count.EqualTo(2));
            Assert.That(free[0].GetCarSymbolName(), Is.EqualTo("some-ident").IgnoreCase);
            Assert.That(free[0].GetSingleKeywordArgument<Symbol>(":name").Name, Is.EqualTo("fk-referenced-table-name-ident").IgnoreCase);
            Assert.That(free[1].GetCarSymbolName(), Is.EqualTo("some-word").IgnoreCase);
            Assert.That(free[1].GetSingleKeywordArgument<Symbol>(":name").Name, Is.EqualTo("fk-referenced-table-name-word").IgnoreCase);

            // block 1
            Assert.That(block1.GetCarSymbolName(), Is.EqualTo("block").IgnoreCase);
            Assert.That(block1.GetSingleKeywordArgument<Symbol>(":ref").Name, Is.EqualTo("fk-referenced-columns").IgnoreCase);
        }
    }
}
