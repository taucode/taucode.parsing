using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Data;
using TauCode.Utils.Extensions;

namespace TauCode.Parsing.Tests.TinyLisp
{
    [TestFixture]
    public class TinyLispExtensionsTests
    {
        private PseudoList _rootList;

        [SetUp]
        public void SetUp()
        {
            var input = this.GetType().Assembly.GetResourceText("sql-grammar.lisp", true);
            ILexer lexer = new TinyLispLexer();

            var tokens = lexer.Lexize(input);

            var reader = new TinyLispPseudoReader();
            _rootList = reader.Read(tokens);
        }

        [Test]
        public void TinyLispExtensions_CreateTable_ProducesValidResults()
        {
            // Arrange

            var defBlock = _rootList[1].AsPseudoList();

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

            var defBlock = _rootList[2];

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
            Assert.That(free[0].GetSingleKeywordArgument<Symbol>(":name").Name,
                Is.EqualTo("column-name-ident").IgnoreCase);
            Assert.That(free[1].GetCarSymbolName(), Is.EqualTo("some-word").IgnoreCase);
            Assert.That(free[1].GetSingleKeywordArgument<Symbol>(":name").Name,
                Is.EqualTo("column-name-word").IgnoreCase);

            // alt1
            Assert.That(alt1.GetCarSymbolName(), Is.EqualTo("alt").IgnoreCase);
            free = alt1.GetFreeArguments();
            Assert.That(free[0].GetCarSymbolName(), Is.EqualTo("some-ident").IgnoreCase);
            Assert.That(free[0].GetSingleKeywordArgument<Symbol>(":name").Name,
                Is.EqualTo("type-name-ident").IgnoreCase);
            Assert.That(free[1].GetCarSymbolName(), Is.EqualTo("some-word").IgnoreCase);
            Assert.That(free[1].GetSingleKeywordArgument<Symbol>(":name").Name,
                Is.EqualTo("type-name-word").IgnoreCase);

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
            Assert.That(innerOptFree[0].GetSingleKeywordArgument<StringAtom>(":value").Value,
                Is.EqualTo(",").IgnoreCase);
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
            Assert.That(free[1].GetSingleKeywordArgument<Symbol>(":name").Name,
                Is.EqualTo("inline-primary-key").IgnoreCase);

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
            Assert.That(innerAltFree[0].GetSingleKeywordArgument<Symbol>(":name").Name,
                Is.EqualTo("default-null").IgnoreCase);
            Assert.That(innerAltFree[1].GetCarSymbolName(), Is.EqualTo("some-int").IgnoreCase);
            Assert.That(innerAltFree[1].GetSingleKeywordArgument<Symbol>(":name").Name,
                Is.EqualTo("default-integer").IgnoreCase);
            Assert.That(innerAltFree[2].GetCarSymbolName(), Is.EqualTo("some-string").IgnoreCase);
            Assert.That(innerAltFree[2].GetSingleKeywordArgument<Symbol>(":name").Name,
                Is.EqualTo("default-string").IgnoreCase);
        }

        [Test]
        public void TinyLispExtensions_PrimaryKey_ProducesValidResults()
        {
            // Arrange
            var defBlock = _rootList[4];

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
        public void TinyLispExtensions_ForeignKey_ProducesValidResults()
        {
            // Arrange
            var defBlock = _rootList[6];

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
            Assert.That(word0.GetSingleKeywordArgument<Symbol>(":name").Name, Is.EqualTo("do-foreign-key").IgnoreCase);

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
            Assert.That(free[0].GetSingleKeywordArgument<Symbol>(":name").Name,
                Is.EqualTo("fk-referenced-table-name-ident").IgnoreCase);
            Assert.That(free[1].GetCarSymbolName(), Is.EqualTo("some-word").IgnoreCase);
            Assert.That(free[1].GetSingleKeywordArgument<Symbol>(":name").Name,
                Is.EqualTo("fk-referenced-table-name-word").IgnoreCase);

            // block 1
            Assert.That(block1.GetCarSymbolName(), Is.EqualTo("block").IgnoreCase);
            Assert.That(block1.GetSingleKeywordArgument<Symbol>(":ref").Name,
                Is.EqualTo("fk-referenced-columns").IgnoreCase);
        }

        [Test]
        public void TinyLispExtensions_ForeignKeyColumns_ProducesValidResults()
        {
            // Arrange
            var defBlock = _rootList[7];

            // Act
            var verb = defBlock.GetCarSymbolName();
            var name = defBlock.GetSingleKeywordArgument<Symbol>(":name").Name;

            var free = defBlock.GetFreeArguments();

            // Assert
            Assert.That(verb, Is.EqualTo("defblock").IgnoreCase);
            Assert.That(name, Is.EqualTo("fk-columns").IgnoreCase);

            Assert.That(free, Has.Count.EqualTo(4));

            var symbol0 = free[0];
            var alt0 = free[1];
            var alt1 = free[2];
            var symbol1 = free[3];

            // symbol 0
            Assert.That(symbol0.GetCarSymbolName(), Is.EqualTo("symbol").IgnoreCase);
            Assert.That(symbol0.GetSingleKeywordArgument<StringAtom>(":value").Value, Is.EqualTo("("));

            // alt 0
            Assert.That(alt0.GetCarSymbolName(), Is.EqualTo("alt").IgnoreCase);
            Assert.That(alt0.GetSingleKeywordArgument<Symbol>(":name").Name,
                Is.EqualTo("fk-column-name-alternatives").IgnoreCase);
            free = alt0.GetFreeArguments();
            Assert.That(free, Has.Count.EqualTo(2));
            Assert.That(free[0].GetCarSymbolName(), Is.EqualTo("some-ident").IgnoreCase);
            Assert.That(free[0].GetSingleKeywordArgument<Symbol>(":name").Name,
                Is.EqualTo("fk-column-name-ident").IgnoreCase);
            Assert.That(free[1].GetCarSymbolName(), Is.EqualTo("some-word").IgnoreCase);
            Assert.That(free[1].GetSingleKeywordArgument<Symbol>(":name").Name,
                Is.EqualTo("fk-column-name-word").IgnoreCase);

            // alt 1
            Assert.That(alt1.GetCarSymbolName(), Is.EqualTo("alt").IgnoreCase);
            free = alt1.GetFreeArguments();
            Assert.That(free, Has.Count.EqualTo(2));
            Assert.That(free[0].GetCarSymbolName(), Is.EqualTo("symbol").IgnoreCase);
            Assert.That(free[0].GetSingleKeywordArgument<StringAtom>(":value").Value, Is.EqualTo(","));
            CollectionAssert.AreEqual(
                new List<Symbol>
                {
                    Symbol.Create("fk-column-name-alternatives")
                },
                free[0].GetAllKeywordArguments(":links")
            );

            Assert.That(free[1].ToString(), Is.EqualTo("(idle)").IgnoreCase);

            // symbol 1
            Assert.That(symbol1.GetCarSymbolName(), Is.EqualTo("symbol").IgnoreCase);
            Assert.That(symbol1.GetSingleKeywordArgument<StringAtom>(":value").Value, Is.EqualTo(")"));
        }

        [Test]
        public void TinyLispExtensions_ForeignKeyReferencedColumns_ProducesValidResults()
        {
            // Arrange
            var defBlock = _rootList[8];

            // Act
            var verb = defBlock.GetCarSymbolName();
            var name = defBlock.GetSingleKeywordArgument<Symbol>(":name").Name;

            var free = defBlock.GetFreeArguments();

            // Assert
            Assert.That(verb, Is.EqualTo("defblock").IgnoreCase);
            Assert.That(name, Is.EqualTo("fk-referenced-columns").IgnoreCase);

            Assert.That(free, Has.Count.EqualTo(4));

            var symbol0 = free[0];
            var alt0 = free[1];
            var alt1 = free[2];
            var symbol1 = free[3];

            // symbol 0
            Assert.That(symbol0.GetCarSymbolName(), Is.EqualTo("symbol").IgnoreCase);
            Assert.That(symbol0.GetSingleKeywordArgument<StringAtom>(":value").Value, Is.EqualTo("("));

            // alt 0
            Assert.That(alt0.GetCarSymbolName(), Is.EqualTo("alt").IgnoreCase);
            Assert.That(alt0.GetSingleKeywordArgument<Symbol>(":name").Name,
                Is.EqualTo("fk-referenced-column-name-alternatives").IgnoreCase);
            free = alt0.GetFreeArguments();
            Assert.That(free, Has.Count.EqualTo(2));
            Assert.That(free[0].GetCarSymbolName(), Is.EqualTo("some-ident").IgnoreCase);
            Assert.That(free[0].GetSingleKeywordArgument<Symbol>(":name").Name,
                Is.EqualTo("fk-referenced-column-name-ident").IgnoreCase);
            Assert.That(free[1].GetCarSymbolName(), Is.EqualTo("some-word").IgnoreCase);
            Assert.That(free[1].GetSingleKeywordArgument<Symbol>(":name").Name,
                Is.EqualTo("fk-referenced-column-name-word").IgnoreCase);

            // alt 1
            Assert.That(alt1.GetCarSymbolName(), Is.EqualTo("alt").IgnoreCase);
            free = alt1.GetFreeArguments();
            Assert.That(free, Has.Count.EqualTo(2));
            Assert.That(free[0].GetCarSymbolName(), Is.EqualTo("symbol").IgnoreCase);
            Assert.That(free[0].GetSingleKeywordArgument<StringAtom>(":value").Value, Is.EqualTo(","));
            CollectionAssert.AreEqual(
                new List<Symbol>
                {
                    Symbol.Create("fk-referenced-column-name-alternatives")
                },
                free[0].GetAllKeywordArguments(":links")
            );

            Assert.That(free[1].ToString(), Is.EqualTo("(idle)").IgnoreCase);

            // symbol 1
            Assert.That(symbol1.GetCarSymbolName(), Is.EqualTo("symbol").IgnoreCase);
            Assert.That(symbol1.GetSingleKeywordArgument<StringAtom>(":value").Value, Is.EqualTo(")"));
        }

        [Test]
        public void AsPseudoList_ElementIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            Element element = null;

            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => element.AsPseudoList());

            // Assert
            Assert.That(ex.ParamName, Is.EqualTo("element"));
        }

        [Test]
        public void AsPseudoList_ArgumentIsNotPseudoList_ThrowsArgumentException()
        {
            // Arrange

            // Act
            var ex = Assert.Throws<ArgumentException>(() => True.Instance.AsPseudoList());

            // Assert
            Assert.That(ex.Message,
                Does.StartWith(
                    $"Argument is expected to be of type '{typeof(PseudoList).FullName}', but was of type '{typeof(True).FullName}'."));
            Assert.That(ex.ParamName, Is.EqualTo("element"));
        }

        [Test]
        public void GetSingleKeywordArgument_ElementIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            Element element = null;

            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => element.GetSingleKeywordArgument(":arg"));

            // Assert
            Assert.That(ex.ParamName, Is.EqualTo("shouldBePseudoList"));
        }

        [Test]
        public void GetSingleKeywordArgument_ElementIsNotPseudoList_ThrowsArgumentException()
        {
            // Arrange
            Element element = True.Instance;

            // Act
            var ex = Assert.Throws<ArgumentException>(() => element.GetSingleKeywordArgument(":arg"));

            // Assert
            Assert.That(ex.Message,
                Does.StartWith("Argument is not of type 'TauCode.Parsing.TinyLisp.Data.PseudoList'."));
            Assert.That(ex.ParamName, Is.EqualTo("shouldBePseudoList"));
        }

        [Test]
        public void GetSingleKeywordArgument_ArgumentNameIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            Element element = new PseudoList();

            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => element.GetSingleKeywordArgument(null));

            // Assert
            Assert.That(ex.ParamName, Is.EqualTo("argumentName"));
        }

        [Test]
        [TestCase("non-keyword")]
        [TestCase("\"some-string\"")]
        public void GetSingleKeywordArgument_ArgumentIsNotKeyword_ThrowsArgumentException(string badKeywordName)
        {
            // Arrange
            Element element = new PseudoList();

            // Act
            var ex = Assert.Throws<ArgumentException>(() => element.GetSingleKeywordArgument(badKeywordName));

            // Assert
            Assert.That(ex.Message, Does.StartWith($"'{badKeywordName}' is not a valid keyword."));
            Assert.That(ex.ParamName, Is.EqualTo("argumentName"));
        }

        [Test]
        public void GetSingleKeywordArgument_ArgumentIsAbsentAbsenceAllowed_ReturnsNull()
        {
            // Arrange
            var formText = "(foo one two :key three)";
            ILexer lexer = new TinyLispLexer();
            var tokens = lexer.Lexize(formText);
            var reader = new TinyLispPseudoReader();
            var pseudoList = reader.Read(tokens);

            // Act
            var notFound = pseudoList.GetSingleKeywordArgument(":non-existing-key", true);

            // Assert
            Assert.That(notFound, Is.Null);
        }

        [Test]
        public void GetSingleKeywordArgument_ArgumentIsAbsentAbsenceNotAllowed_ThrowsTinyLispException()
        {
            // Arrange
            var formText = "(foo one two :key three)";
            ILexer lexer = new TinyLispLexer();
            var tokens = lexer.Lexize(formText);
            var reader = new TinyLispPseudoReader();
            var pseudoList = reader.Read(tokens).Single().AsPseudoList();

            // Act
            var ex = Assert.Throws<TinyLispException>(() => pseudoList.GetSingleKeywordArgument(":non-existing-key"));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("No argument for keyword ':non-existing-key'."));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void GetSingleKeywordArgument_ArgumentNameIsPresentButIsAKeyword_ThrowsTinyLispException(
            bool absenceIsAllowed)
        {
            // Arrange
            var formText = "(foo one two :key three :your-key :no-luck)";
            ILexer lexer = new TinyLispLexer();
            var tokens = lexer.Lexize(formText);
            var reader = new TinyLispPseudoReader();
            var pseudoList = reader.Read(tokens).Single().AsPseudoList();

            // Act
            var ex = Assert.Throws<TinyLispException>(() =>
                pseudoList.GetSingleKeywordArgument(":your-key", absenceIsAllowed));

            // Assert
            Assert.That(ex.Message,
                Is.EqualTo("Keyword ':your-key' was found, but next element is a keyword too."));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void GetSingleKeywordArgument_ArgumentNameIsPresentButIsAtEnd_ThrowsTinyLispException(
            bool absenceIsAllowed)
        {
            // Arrange
            var formText = "(foo one two :key three :your-key)";
            ILexer lexer = new TinyLispLexer();
            var tokens = lexer.Lexize(formText);
            var reader = new TinyLispPseudoReader();
            var pseudoList = reader.Read(tokens).Single().AsPseudoList();

            // Act
            var ex = Assert.Throws<TinyLispException>(() =>
                pseudoList.GetSingleKeywordArgument(":your-key", absenceIsAllowed));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Keyword ':your-key' was found, but at the end of the list."));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void GetSingleKeywordArgumentGeneric_ArgumentNameIsPresentButArgumentIsOfWrongType_ThrowsTinyLispException(bool absenceIsAllowed)
        {
            // Arrange
            var formText = "(foo one two :key three :your-key \"some string\")";
            ILexer lexer = new TinyLispLexer();
            var tokens = lexer.Lexize(formText);
            var reader = new TinyLispPseudoReader();
            var pseudoList = reader.Read(tokens).Single().AsPseudoList();

            // Act
            var ex = Assert.Throws<TinyLispException>(() =>
                pseudoList.GetSingleKeywordArgument<Symbol>(":your-key", absenceIsAllowed));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Argument for ':your-key' was found, but it appears to be of type 'TauCode.Parsing.TinyLisp.Data.StringAtom' instead of expected type 'TauCode.Parsing.TinyLisp.Data.Symbol'."));
        }
    }
}
