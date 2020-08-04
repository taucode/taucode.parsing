using NUnit.Framework;
using System;
using System.Collections.Generic;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Tests.TinyLisp
{
    [TestFixture]
    public class PseudoListTests
    {
        [Test]
        public void Constructor_ElementsArray_CreatesExpectedInstance()
        {
            // Arrange
            var elements = new Element[]
            {
                Nil.Instance,
                True.Instance,
                Symbol.Create("aha"),
                Symbol.Create(":key"),
            };

            // Act
            var pseudoList = new PseudoList(elements);

            // Assert
            CollectionAssert.AreEqual(pseudoList, elements);
        }

        [Test]
        public void Constructor_ElementsArrayIsNull_ThrowsArgumentNullException()
        {
            // Arrange

            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => new PseudoList(null));

            // Assert
            Assert.That(ex.ParamName, Is.EqualTo("elements"));
        }

        [Test]
        public void Constructor_ElementsArrayContainsNulls_ThrowsArgumentException()
        {
            // Arrange
            var elements = new Element[]
            {
                Nil.Instance,
                True.Instance,
                null,
                Symbol.Create("aha"),
                Symbol.Create(":key"),
            };

            // Act
            var ex = Assert.Throws<ArgumentException>(() => new PseudoList(elements));

            // Assert
            Assert.That(ex.Message, Does.StartWith("'elements' must not contain nulls."));
            Assert.That(ex.ParamName, Is.EqualTo("elements"));
        }

        [Test]
        public void AddElement_ArgumentIsValid_AddsElement()
        {
            // Arrange
            var elements = new Element[]
            {
                Nil.Instance,
                True.Instance,
                Symbol.Create("aha"),
                Symbol.Create(":key"),
            };

            var pseudoList = new PseudoList(elements);

            // Act
            var anotherSymbol = Symbol.Create("another");
            pseudoList.AddElement(anotherSymbol);

            // Assert
            var expectedList = new List<Element>(elements)
            {
                anotherSymbol
            };

            CollectionAssert.AreEqual(expectedList, pseudoList);
        }

        [Test]
        public void AddElement_ArgumentIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var elements = new Element[]
            {
                Nil.Instance,
                True.Instance,
                Symbol.Create("aha"),
                Symbol.Create(":key"),
            };

            var pseudoList = new PseudoList(elements);

            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => pseudoList.AddElement(null));

            // Assert
            Assert.That(ex.ParamName, Is.EqualTo("element"));
        }

    }
}
