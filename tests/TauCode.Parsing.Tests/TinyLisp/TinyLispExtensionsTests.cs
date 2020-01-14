using NUnit.Framework;
using System;
using System.Linq;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Omicron;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Tests.TinyLisp
{
    [TestFixture]
    public class TinyLispExtensionsTests
    {
        private ILexer _lexer;

        [SetUp]
        public void SetUp()
        {
            _lexer = new /*Tiny-LispLexer()*/ OmicronTinyLispLexer();
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
            
            var tokens = _lexer.Lexize(formText);
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
            
            var tokens = _lexer.Lexize(formText);
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
            
            var tokens = _lexer.Lexize(formText);
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
            
            var tokens = _lexer.Lexize(formText);
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
            
            var tokens = _lexer.Lexize(formText);
            var reader = new TinyLispPseudoReader();
            var pseudoList = reader.Read(tokens).Single().AsPseudoList();

            // Act
            var ex = Assert.Throws<TinyLispException>(() =>
                pseudoList.GetSingleKeywordArgument<Symbol>(":your-key", absenceIsAllowed));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Argument for ':your-key' was found, but it appears to be of type 'TauCode.Parsing.TinyLisp.Data.StringAtom' instead of expected type 'TauCode.Parsing.TinyLisp.Data.Symbol'."));
        }

        [Test]
        public void GetAllKeywordArguments_ValidArguments_ReturnsExpectedResult()
        {
            // Arrange
            var formText = "(foo one two :key one two \"three\" :your-key \"some string\")";
            
            var tokens = _lexer.Lexize(formText);
            var reader = new TinyLispPseudoReader();
            var pseudoList = reader.Read(tokens).Single().AsPseudoList();

            // Act
            var args = pseudoList.GetAllKeywordArguments(":key");

            // Assert
            CollectionAssert.AreEqual(
                new Element[]
                {
                    Symbol.Create("one"),
                    Symbol.Create("two"),
                    new StringAtom("three"),
                },
                args);
        }

        [Test]
        public void GetAllKeywordArguments_NoSuchKeyAbsenceIsAllowed_ReturnsEmptyPseudoList()
        {
            // Arrange
            var formText = "(foo one two :key one two \"three\" :your-key \"some string\")";
            
            var tokens = _lexer.Lexize(formText);
            var reader = new TinyLispPseudoReader();
            var pseudoList = reader.Read(tokens).Single().AsPseudoList();

            // Act
            var args = pseudoList.GetAllKeywordArguments(":non-existing-key", true);

            // Assert
            Assert.That(args, Is.Empty);
        }

        [Test]
        public void GetAllKeywordArguments_NoSuchKeyAbsenceNotAllowed_ThrowsTinyLispException()
        {
            // Arrange
            var formText = "(foo one two :key one two \"three\" :your-key \"some string\")";
            
            var tokens = _lexer.Lexize(formText);
            var reader = new TinyLispPseudoReader();
            var pseudoList = reader.Read(tokens).Single().AsPseudoList();

            // Act
            var ex = Assert.Throws<TinyLispException>(() =>
                pseudoList.GetAllKeywordArguments(":non-existing-key", false));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("No argument for keyword ':non-existing-key'."));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void GetAllKeywordArguments_KeywordFoundButNextItemIsAlsoKeyword_ReturnsEmptyPseudoList(bool absenceIsAllowed)
        {
            // Arrange
            var formText = "(foo one two :key one two \"three\" :your-key :no-items-for-you \"some string\")";
            
            var tokens = _lexer.Lexize(formText);
            var reader = new TinyLispPseudoReader();
            var pseudoList = reader.Read(tokens).Single().AsPseudoList();

            // Act
            var args = pseudoList.GetAllKeywordArguments(":your-key");

            // Assert
            Assert.That(args, Is.Empty);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void GetAllKeywordArguments_KeywordFoundButAtEnd_ReturnsEmptyPseudoList(bool absenceIsAllowed)
        {
            // Arrange
            var formText = "(foo one two :key one two \"three\" :no-items-for-you \"some string\" :your-key-at-end)";
            
            var tokens = _lexer.Lexize(formText);
            var reader = new TinyLispPseudoReader();
            var pseudoList = reader.Read(tokens).Single().AsPseudoList();

            // Act
            var args = pseudoList.GetAllKeywordArguments(":your-key-at-end");

            // Assert
            Assert.That(args, Is.Empty);
        }

        [Test]
        public void GetAllKeywordArguments_PseudoListIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            PseudoList pseudoList = null;

            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => pseudoList.GetAllKeywordArguments(":key"));

            // Assert
            Assert.That(ex.ParamName, Is.EqualTo("shouldBePseudoList"));
        }

        [Test]
        public void GetAllKeywordArguments_FirstArgumentIsNotPseudoList_ThrowsArgumentException()
        {
            // Arrange
            var symbol = Symbol.Create("hello");

            // Act
            var ex = Assert.Throws<ArgumentException>(() => symbol.GetAllKeywordArguments(":key"));

            // Assert
            Assert.That(ex.Message,
                Does.StartWith("Argument is not of type 'TauCode.Parsing.TinyLisp.Data.PseudoList'."));
            Assert.That(ex.ParamName, Is.EqualTo("shouldBePseudoList"));
        }

        [Test]
        public void GetAllKeywordArguments_ArgumentIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var formText = "(foo one two :key one two \"three\" :your-key \"some string\")";
            
            var tokens = _lexer.Lexize(formText);
            var reader = new TinyLispPseudoReader();
            var pseudoList = reader.Read(tokens).Single().AsPseudoList();

            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => pseudoList.GetAllKeywordArguments(null));

            // Assert
            Assert.That(ex.ParamName, Is.EqualTo("argumentName"));
        }

        [Test]
        [TestCase("non-keyword")]
        [TestCase("\"some-string\"")]
        public void GetAllKeywordArguments_ArgumentIsNotKeyword_ThrowsArgumentException(string badKeywordName)
        {
            // Arrange
            var formText = "(foo one two :key one two \"three\" :your-key \"some string\")";
            
            var tokens = _lexer.Lexize(formText);
            var reader = new TinyLispPseudoReader();
            var pseudoList = reader.Read(tokens).Single().AsPseudoList();


            // Act
            var ex = Assert.Throws<ArgumentException>(() => pseudoList.GetAllKeywordArguments(badKeywordName));

            // Assert
            Assert.That(ex.Message, Does.StartWith($"'{badKeywordName}' is not a valid keyword."));
            Assert.That(ex.ParamName, Is.EqualTo("argumentName"));
        }

        [Test]
        public void GetSingleArgumentAsBool_ItemNotFound_ReturnsNull()
        {
            // Arrange
            var formText = "(foo one two :key one two \"three\" :your-key \"some string\")";
            
            var tokens = _lexer.Lexize(formText);
            var reader = new TinyLispPseudoReader();
            var pseudoList = reader.Read(tokens).Single().AsPseudoList();

            // Act
            var res = pseudoList.GetSingleArgumentAsBool(":non-existing-key");

            // Assert
            Assert.That(res, Is.Null);
        }

        [Test]
        public void GetSingleArgumentAsBool_ItemIsNil_ReturnsFalse()
        {
            // Arrange
            var formText = "(foo one two :key nil one two \"three\" :your-key \"some string\")";
            
            var tokens = _lexer.Lexize(formText);
            var reader = new TinyLispPseudoReader();
            var pseudoList = reader.Read(tokens).Single().AsPseudoList();

            // Act
            var res = pseudoList.GetSingleArgumentAsBool(":key");

            // Assert
            Assert.That(res, Is.False);
        }

        [Test]
        public void GetSingleArgumentAsBool_ItemIsTrue_ReturnsFalse()
        {
            // Arrange
            var formText = "(foo one two :key t one two \"three\" :your-key \"some string\")";
            
            var tokens = _lexer.Lexize(formText);
            var reader = new TinyLispPseudoReader();
            var pseudoList = reader.Read(tokens).Single().AsPseudoList();

            // Act
            var res = pseudoList.GetSingleArgumentAsBool(":key");

            // Assert
            Assert.That(res, Is.True);
        }

        [Test]
        [TestCase("some-symbol")]
        [TestCase("\"a string\"")]
        public void GetSingleArgumentAsBool_ItemIsOfInvalidType_ThrowsTinyLispException(string badItem)
        {
            // Arrange
            var formText = $"(foo one two :key {badItem} one two \"three\" :your-key \"some string\")";
            
            var tokens = _lexer.Lexize(formText);
            var reader = new TinyLispPseudoReader();
            var pseudoList = reader.Read(tokens).Single().AsPseudoList();

            // Act
            var ex = Assert.Throws<TinyLispException>(() => pseudoList.GetSingleArgumentAsBool(":key"));

            // Assert
            var wrongItem = reader.Read(_lexer.Lexize(badItem)).Single().ToString();
            Assert.That(ex.Message, Is.EqualTo($"Keyword ':key' was found, but it appeared to be '{wrongItem}' instead of NIL or T."));
        }

        [Test]
        [TestCase("(defun f (x) (* x x))", "DEFUN")]
        [TestCase("(nil t)", "NIL")]
        [TestCase("(t nil)", "T")]
        public void GetCarSymbolName_HappyPath_ReturnsExpectedResult(string form, string expectedCar)
        {
            // Arrange
            
            var tokens = _lexer.Lexize(form);
            var reader = new TinyLispPseudoReader();
            var pseudoList = reader.Read(tokens).Single().AsPseudoList();

            // Act
            var car = pseudoList.GetCarSymbolName();

            // Assert
            Assert.That(car, Is.EqualTo(expectedCar));
        }

        [Test]
        public void GetCarSymbolName_ArgumentIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            PseudoList pseudoList = null;

            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => pseudoList.GetCarSymbolName());

            // Assert
            Assert.That(ex.ParamName, Is.EqualTo("shouldBePseudoList"));
        }

        [Test]
        public void GetCarSymbolName_ArgumentIsNotPseudoList_ThrowsArgumentException()
        {
            // Arrange
            var element = Nil.Instance;

            // Act
            var ex = Assert.Throws<ArgumentException>(() => element.GetCarSymbolName());

            // Assert
            Assert.That(ex.Message, Does.StartWith("Argument is not of type 'TauCode.Parsing.TinyLisp.Data.PseudoList'."));
            Assert.That(ex.ParamName, Is.EqualTo("shouldBePseudoList"));
        }

        [Test]
        public void GetCarSymbolName_PseudoListIsEmpty_ThrowsArgumentException()
        {
            // Arrange
            var element = new PseudoList();

            // Act
            var ex = Assert.Throws<ArgumentException>(() => element.GetCarSymbolName());

            // Assert
            Assert.That(ex.Message, Does.StartWith("PseudoList is empty."));
            Assert.That(ex.ParamName, Is.EqualTo("shouldBePseudoList"));
        }

        [Test]
        public void GetCarSymbolName_CarIsNotSymbol_ThrowsArgumentException()
        {
            // Arrange
            var element = new PseudoList(new[] { new StringAtom("some string"), });

            // Act
            var ex = Assert.Throws<ArgumentException>(() => element.GetCarSymbolName());

            // Assert
            Assert.That(ex.Message, Does.StartWith("CAR of PseudoList is not a symbol."));
            Assert.That(ex.ParamName, Is.EqualTo("shouldBePseudoList"));
        }

        [Test]
        [TestCase("(form :a non-free free1 free2 :b :c non-free-2 next-free)", "((free1 free2) (next-free))")]
        [TestCase("(form free1 :b :c non-free-2 next-free-1 next-free-2)", "((free1) (next-free-1 next-free-2))")]
        [TestCase("(form free1 :b :c non-free-2)", "((free1))")]
        public void GetMultipleFreeArgumentSets_HappyPath_ReturnsExpectedResult(string form, string expectedRepresentation)
        {
            // Arrange
            
            var tokens = _lexer.Lexize(form);
            var reader = new TinyLispPseudoReader();
            var pseudoList = reader.Read(tokens).Single().AsPseudoList();
            
            // Act
            var list = pseudoList.GetMultipleFreeArgumentSets();
            var listToPseudoList = new PseudoList(list);

            // Assert
            Assert.That(listToPseudoList.ToString(), Is.EqualTo(expectedRepresentation).IgnoreCase);
        }

        [Test]
        public void GetMultipleFreeArgumentSets_ArgumentIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            PseudoList pseudoList = null;

            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => pseudoList.GetMultipleFreeArgumentSets());

            // Assert
            Assert.That(ex.ParamName, Is.EqualTo("shouldBePseudoList"));
        }

        [Test]
        public void GetMultipleFreeArgumentSets_ArgumentIsNotPseudoList_ThrowsArgumentException()
        {
            // Arrange
            var element = Nil.Instance;

            // Act
            var ex = Assert.Throws<ArgumentException>(() => element.GetMultipleFreeArgumentSets());

            // Assert
            Assert.That(ex.Message, Does.StartWith("Argument is not of type 'TauCode.Parsing.TinyLisp.Data.PseudoList'."));
            Assert.That(ex.ParamName, Is.EqualTo("shouldBePseudoList"));
        }

        [Test]
        [TestCase("(form :a non-free :b :c non-free-2 next-free)", "(next-free)")]
        [TestCase("(form free1 :b :c non-free-2)", "(free1)")]
        public void GetFreeArguments_HappyPath_ReturnsExpectedResult(string form, string expectedRepresentation)
        {
            // Arrange
            
            var tokens = _lexer.Lexize(form);
            var reader = new TinyLispPseudoReader();
            var pseudoList = reader.Read(tokens).Single().AsPseudoList();

            // Act
            var argsPseudoList = pseudoList.GetFreeArguments();

            // Assert
            Assert.That(argsPseudoList.ToString(), Is.EqualTo(expectedRepresentation).IgnoreCase);
        }

        [Test]
        [TestCase("(form :a non-free :b :c non-free-2)")]
        [TestCase("(form :akka free1 :b :c non-free-2)")]
        public void GetFreeArguments_NoFreeArgs_ThrowsTinyLispException(string form)
        {
            // Arrange
            
            var tokens = _lexer.Lexize(form);
            var reader = new TinyLispPseudoReader();
            var pseudoList = reader.Read(tokens).Single().AsPseudoList();

            // Act
            var ex = Assert.Throws<TinyLispException>(() => pseudoList.GetFreeArguments());

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Free arguments not found."));
        }

        [Test]
        [TestCase("(form :a non-free free1 free2 :b :c non-free-2 next-free)")]
        [TestCase("(form free1 :b :c non-free-2 next-free-1 next-free-2)")]
        public void GetFreeArguments_MoreThanOneFreeArgSet_ThrowsTinyLispException(string form)
        {
            // Arrange
            
            var tokens = _lexer.Lexize(form);
            var reader = new TinyLispPseudoReader();
            var pseudoList = reader.Read(tokens).Single().AsPseudoList();

            // Act
            var ex = Assert.Throws<TinyLispException>(() => pseudoList.GetFreeArguments());

            // Assert
            Assert.That(ex.Message, Is.EqualTo("More than one set of free arguments was found."));
        }
    }
}
