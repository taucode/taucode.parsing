using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.TextProcessing;

namespace TauCode.Parsing.Lexing.StandardEscapeProcessors
{
    public class CLangSingleCharEscapeProcessor : EscapeProcessorBase
    {
        private static readonly string[] ReplacementStrings =
        {
            "\"\"",
            "\\\\",
            "0\0",
            "a\a",
            "b\b",
            "f\f",
            "n\n",
            "r\r",
            "t\t",
            "v\v",
        };

        private static readonly Dictionary<char, char> Replacements;

        private char? GetReplacement(char escape)
        {
            if (Replacements.TryGetValue(escape, out var replacement))
            {
                return replacement;
            }

            return null;
        }

        static CLangSingleCharEscapeProcessor()
        {
            Replacements = ReplacementStrings
                .ToDictionary(
                    x => x.First(),
                    x => x.Skip(1).Single());
        }

        public CLangSingleCharEscapeProcessor()
            : base('\\', false)
        {
        }

        protected override EscapePayload DeliverPayloadImpl()
        {
            var c = this.Context.GetCurrentChar();

            var replacement = GetReplacement(c);
            if (replacement.HasValue)
            {
                this.Context.AdvanceByChar();
                return new EscapePayload(replacement.Value, null);
            }

            return null;
        }
    }
}
