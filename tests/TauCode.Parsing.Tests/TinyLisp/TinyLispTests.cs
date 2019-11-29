using NUnit.Framework;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Tests.TinyLisp
{
    [TestFixture]
    public class TinyLispTests
    {
        [Test]
        public void Wat()
        {
            // Arrange

            // Act
            var wat = Symbol.Create("ui");
            var wat2 = Symbol.Create("Ui");

            var kek = Symbol.Create(":10-dd");
            var kek2 = Symbol.Create(":10-Dd");

            var s = Symbol.Create("aaa");
            var sk = Symbol.Create(":aaa");

            // Assert
            var pp = ReferenceEquals(wat, wat2);
            Assert.That(pp);

            Assert.That(wat == wat2);
            Assert.That(ReferenceEquals(wat, wat2));

            Assert.That(kek == kek2);
            Assert.That(ReferenceEquals(kek, kek2));

            Assert.That(s != sk);
        }
    }
}
