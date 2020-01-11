using NUnit.Framework;
using System;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Old.Tests.TinyLisp
{
    [TestFixture]
    public class SymbolTests
    {
        [Test]
        public void Create_DifferentArguments_ProducesExpectedResult()
        {
            // Arrange

            // Act
            var symbol1 = Symbol.Create("ui");
            var symbol1Alternative = Symbol.Create("Ui");

            var keyword1 = Symbol.Create(":10-dd");
            var keyword1Alternative = Symbol.Create(":10-Dd");

            var symbol2 = Symbol.Create("aaa");
            var symbol2ButKeyword = Symbol.Create(":aaa");

            // Assert
            var shouldBe = ReferenceEquals(symbol1, symbol1Alternative);
            Assert.That(shouldBe);

            Assert.That(symbol1 == symbol1Alternative);
            Assert.That(ReferenceEquals(symbol1, symbol1Alternative));

            Assert.That(keyword1 == keyword1Alternative);
            Assert.That(ReferenceEquals(keyword1, keyword1Alternative));

            var myNil = Symbol.Create("nil");
            var myT = Symbol.Create("t");

            Assert.That(Nil.Instance, Is.SameAs(myNil));
            Assert.That(True.Instance, Is.SameAs(myT));

            Assert.That(symbol2 != symbol2ButKeyword);
        }

        [Test]
        public void Create_NullSymbolName_ThrowsTinyLispException()
        {
            // Arrange

            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => Symbol.Create(null));

            // Assert
            Assert.That(ex.ParamName, Is.EqualTo("name"));
        }

        [Test]
        public void Create_EmptySymbolName_ThrowsTinyLispException()
        {
            // Arrange

            // Act
            var ex = Assert.Throws<ArgumentException>(() => Symbol.Create(""));

            // Assert
            Assert.That(ex.Message, Does.StartWith("Symbol name cannot be empty."));
            Assert.That(ex.ParamName, Is.EqualTo("name"));
        }

        [Test]
        [TestCase("the.symbol")]
        [TestCase("symbol ")]
        [TestCase(" symbol")]
        [TestCase("1488", Description = "Integer is not a valid symbol name.")]
        public void Create_InvalidSymbolName_ThrowsTinyLispException(string badSymbolName)
        {
            // Arrange

            // Act
            var ex = Assert.Throws<ArgumentException>(() => Symbol.Create(badSymbolName));

            // Assert
            Assert.That(ex.Message, Does.StartWith($"Invalid symbol name: '{badSymbolName}'."));
            Assert.That(ex.ParamName, Is.EqualTo("name"));
        }

        [Test]
        [TestCase(":")]
        [TestCase(":ab ")]
        [TestCase(": ab")]
        [TestCase(":a:d")]
        public void Create_InvalidKeywordName_ThrowsTinyLispException(string badKeywordName)
        {
            // Arrange

            // Act
            var ex = Assert.Throws<ArgumentException>(() => Symbol.Create(badKeywordName));

            // Assert
            Assert.That(ex.Message, Does.StartWith($"Invalid keyword name: '{badKeywordName}'."));
            Assert.That(ex.ParamName, Is.EqualTo("name"));
        }
    }
}
