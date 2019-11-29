using System.Collections.Generic;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Lexer2
{
    public class WordExtractor : TokenExtractorBase
    {
        private static readonly char[] WordFirstChars = GetWordFirstChars();
        private static readonly HashSet<char> InnerChars = new HashSet<char>(GetWordFirstChars());

        private static char[] GetWordFirstChars()
        {
            var list = new List<char>();
            list.Add('_');
            list.AddCharRange('a', 'z');
            list.AddCharRange('A', 'Z');
            list.AddCharRange('0', '9');

            return list.ToArray();
        }

        public WordExtractor(char[] spaceChars)
            : base(spaceChars, WordFirstChars)
        {
        }

        protected override void Reset()
        {
            //throw new NotImplementedException();
        }

        protected override IToken ProduceResult()
        {
            var resultString = this.ExtractResultString();
            return new WordToken(resultString);
        }

        protected override TestCharResult TestCurrentChar()
        {
            var localPos = this.GetLocalPosition();
            var c = this.GetCurrentChar();

            if (localPos == 0)
            {
                return TestCharResult.Continue;
            }

            if (InnerChars.Contains(c))
            {
                return TestCharResult.Continue;
            }

            return TestCharResult.End;
        }
    }
}
