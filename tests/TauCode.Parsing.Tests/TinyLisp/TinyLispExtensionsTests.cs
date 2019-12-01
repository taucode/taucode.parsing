using NUnit.Framework;
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

            Assert.That(word.GetSingleKeywordArgument<StringAtom>(":value").Value, Is.EqualTo("TABLE"));

            Assert.That(alt.GetCarSymbolName(), Is.EqualTo("alt").IgnoreCase);
        }
    }
}
