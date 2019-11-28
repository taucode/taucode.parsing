using System;
using System.Collections.Generic;

namespace TauCode.Parsing.Lexer2
{
    public class WordExtractor : TokenExtractorBase
    {
        private static readonly char[] WordFirstChars = GetWordFirstChars();

        private static char[] GetWordFirstChars()
        {
            var list = new List<char>();
            list.Add('_');
            list.AddCharRange('a', 'z');
            list.AddCharRange('A', 'Z');
            list.AddCharRange('0', '9');

            return list.ToArray();
        }

        public WordExtractor()
            : base(WordFirstChars)
        {
        }

        protected override void Reset()
        {
            throw new NotImplementedException();
        }

        protected override IToken ProduceResult()
        {
            throw new NotImplementedException();
        }

        protected override TestCharResult TestCurrentChar()
        {
            throw new NotImplementedException();
        }
    }
}
