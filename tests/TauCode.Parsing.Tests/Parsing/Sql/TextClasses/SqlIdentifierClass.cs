﻿using System.Collections.Generic;
using System.Linq;
using TauCode.Extensions;
using TauCode.Parsing.Building;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.TextClasses;
using TauCode.Parsing.TinyLisp;

namespace TauCode.Parsing.Tests.Parsing.Sql.TextClasses
{
    [TextClass("identifier")]
    public class SqlIdentifierClass : TextClassBase
    {
        //private readonly HashSet<string> _reservedWords;

        public static SqlIdentifierClass Instance { get; } = new SqlIdentifierClass();

        private SqlIdentifierClass(/*IList<string> reservedWords*/)
        {
            //throw new NotImplementedException();
            //_reservedWords = new HashSet<string>(reservedWords.Select(x => x.ToUpperInvariant()));
        }

        protected override string TryConvertFromImpl(string text, ITextClass anotherClass)
        {
            var todo = SqlTestsHelper.ReservedWords.OrderBy(x => x).ToList();

            if (
                anotherClass is WordTextClass &&
                !SqlTestsHelper.IsReservedWord(text))
            {
                return text;
            }

            return null;
        }
    }

    public static class SqlTestsHelper
    {
        private static readonly ILexer _tinyLispLexer = new TinyLispLexer();
        private static HashSet<string> _reservedWords;

        public static HashSet<string> ReservedWords = _reservedWords ?? (_reservedWords = CreateReservedWords());

        private static HashSet<string> CreateReservedWords()
        {
            var grammar = typeof(SqlTestsHelper).Assembly.GetResourceText("sql-grammar.lisp", true);
            var tokens = _tinyLispLexer.Lexize(grammar);

            var reader = new TinyLispPseudoReader();
            var form = reader.Read(tokens);

            var nodeFactory = new SqlNodeFactory();
            var builder = new Builder();
            var root = builder.Build(nodeFactory, form);
            var nodes = root.FetchTree();

            var words = new List<string>();

            words.AddRange(nodes
                .Where(x => x is ExactTextNode)
                .Cast<ExactTextNode>()
                .Select(x => x.ExactText.ToLowerInvariant()));

            words.AddRange(nodes
                .Where(x => x is MultiTextNode)
                .Cast<MultiTextNode>()
                .SelectMany(x => x.Texts.Select(y => y.ToLowerInvariant())));

            return new HashSet<string>(words);
        }

        public static bool IsReservedWord(string text) => ReservedWords.Contains(text.ToLowerInvariant());
    }
}