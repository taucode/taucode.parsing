using NUnit.Framework;
using System;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Tests.TinyLisp
{
    [TestFixture]
    public class PseudoListTests
    {
        [Test]
        public void Create_NullSymbolName_ThrowsTinyLispException()
        {
            // Arrange
            var k = new PseudoList();

            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => Symbol.Create(null));

            // Assert
            Assert.That(ex.ParamName, Is.EqualTo("name"));
        }


    }
}
