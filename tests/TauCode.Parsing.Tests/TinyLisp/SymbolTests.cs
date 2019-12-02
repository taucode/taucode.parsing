using NUnit.Framework;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Tests.TinyLisp
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

    }
}
