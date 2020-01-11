using System.Collections.Generic;
using System.Linq;
using TauCode.Extensions;
using TauCode.Parsing.Building;
using TauCode.Parsing.Lab;
using TauCode.Parsing.Lab.Nodes;
using TauCode.Parsing.Lab.TextClasses;
using TauCode.Parsing.Lexing;

namespace TauCode.Parsing.Tests.Parsing.Sql.TextClasses
{
    [TextClass("identifier")]
    public class SqlIdentifierClass : TextClassBaseLab
    {
        //private readonly HashSet<string> _reservedWords;

        public static SqlIdentifierClass Instance { get; } = new SqlIdentifierClass();

        private SqlIdentifierClass(/*IList<string> reservedWords*/)
        {
            //throw new NotImplementedException();
            //_reservedWords = new HashSet<string>(reservedWords.Select(x => x.ToUpperInvariant()));
        }

        protected override string TryConvertFromImpl(string text, ITextClassLab anotherClass)
        {
            var todo = SqlTestsHelper.ReservedWords.OrderBy(x => x).ToList();

            if (
                anotherClass is WordTextClassLab &&
                !SqlTestsHelper.IsReservedWord(text))
            {
                return text;
            }

            return null;
        }
    }

    public static class SqlTestsHelper
    {
        private static readonly ILexer _tinyLispLexer = new TinyLispLexerLab();
        private static HashSet<string> _reservedWords;

        public static HashSet<string> ReservedWords = _reservedWords ?? (_reservedWords = CreateReservedWords());

        private static HashSet<string> CreateReservedWords()
        {
            var grammar = typeof(SqlTestsHelper).Assembly.GetResourceText("sql-grammar.lisp", true);
            var tokens = _tinyLispLexer.Lexize(grammar);

            var reader = new TinyLispPseudoReaderLab();
            var form = reader.Read(tokens);

            var nodeFactory = new SqlNodeFactory();
            var builder = new Builder();
            var root = builder.Build(nodeFactory, form);
            var nodes = root.FetchTree();

            var words = new List<string>();

            words.AddRange(nodes
                .Where(x => x is ExactTextNodeLab)
                .Cast<ExactTextNodeLab>()
                .Select(x => x.ExactText.ToLowerInvariant()));

            words.AddRange(nodes
                .Where(x => x is MultiTextNodeLab)
                .Cast<MultiTextNodeLab>()
                .SelectMany(x => x.Texts.Select(y => y.ToLowerInvariant())));

            return new HashSet<string>(words);
        }

        public static bool IsReservedWord(string text) => ReservedWords.Contains(text.ToLowerInvariant());
    }
}
