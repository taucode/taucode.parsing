using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Lexizing;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Data;
using TauCode.Utils.Extensions;

namespace TauCode.Parsing.Tests.TinyLisp
{
    [TestFixture]
    public class TinyLispTests
    {
        [Test]
        public void TodoWat6_Extensions()
        {
            // Arrange
            var input = this.GetType().Assembly.GetResourceText("sql-grammar.lisp", true);
            ILexer lexer = new TinyLispLexer();

            var tokens = lexer.Lexize(input);

            var reader = new TinyLispPseudoReader();
            var list = reader.Read(tokens);

            var create = list[0] as PseudoList;

            // Act
            var blockName = create.GetSingleKeywordArgument(":name", false) as Symbol;
            var isTop = create.GetSingleArgumentAsBool(":is-top");

            // Assert
            Assert.That(blockName.Name, Is.EqualTo("create").IgnoreCase);
            Assert.That(isTop, Is.True);
        }

        [Test]
        public void TodoWat7_GetFreeArgumentSets()
        {
            // Arrange
            var input = @"

(defblock free1 free2 free3
    :name create-table :is-top nil :is-good t
    (word :value ""TABLE"")
    (alt (some-ident :name table-name-ident) (some-word :name first-name second-name third-name))
    (symbol :value ""("")
    (block :ref column-def :links table-closing next back :ker)
    (symbol :value "","" :links column-def next)
    (block :ref constraint-defs)
    (symbol :value "")"" :name table-closing)
    :breaker some-value
    item1
    item2
    item3
)

";
            ILexer lexer = new TinyLispLexer();

            var tokens = lexer.Lexize(input);

            var reader = new TinyLispPseudoReader();
            var list = reader.Read(tokens);

            var form = list.Single() as PseudoList;

            // Act
            var formVerb = form.GetCarSymbolName();
            var formNameSymbol = form.GetSingleKeywordArgument(":name") as Symbol;
            var isTop = form.GetSingleArgumentAsBool(":is-top");
            var isGood = form.GetSingleArgumentAsBool(":is-good");
            var isWat = form.GetSingleArgumentAsBool(":is-wat");
            var freeArgSet = form.GetMultipleFreeArgumentSets(); // todo: ut these args
            var alt = freeArgSet[1][1];
            var names = alt.GetPseudoLast().AsPseudoList().GetAllKeywordArguments(":name");

            // Assert
            Assert.That(formVerb, Is.EqualTo("defblock").IgnoreCase);
            Assert.That(formNameSymbol.Name, Is.EqualTo("create-table").IgnoreCase);
            Assert.That(isTop, Is.False);
            Assert.That(isGood, Is.True);
            Assert.That(isWat, Is.Null);

            CollectionAssert.AreEqual(
                names,
                new List<Symbol>
                {
                    Symbol.Create("first-name"),
                    Symbol.Create("second-name"),
                    Symbol.Create("third-name")
                });


            throw new NotImplementedException("good, go on!");
        }
    }
}
