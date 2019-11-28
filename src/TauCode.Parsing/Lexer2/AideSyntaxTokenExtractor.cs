using System;
using System.Collections.Generic;

namespace TauCode.Parsing.Lexer2
{
    public class AideSyntaxTokenExtractor : TokenExtractorBase
    {
        private static readonly char[] FirstChars = { '\\' };
        private static readonly HashSet<char> SymbolChars = new HashSet<char>(GetSymbolChars());
        private static readonly HashSet<char> AlphabeticChars = new HashSet<char>(GetAlphabeticChars());
        private static readonly HashSet<char> SecondChars = new HashSet<char>(GetSecondChars());

        private static char[] GetSymbolChars()
        {
            return new[]
            {
                '~',
                '?',
                '!',
                '@',
                '#',
                '$',
                '%',
                '^',
                '&',
                '|',
                '*',
                '/',
                '+',
                '-',
                '(',
                ')',
                '[',
                ']',
                '{',
                '}',
                '.',
                ',',
                ':',
                ';',
                '\\',
            };
        }

        private static char[] GetAlphabeticChars()
        {
            var list = new List<char>();
            list.AddCharRange('a', 'z');
            list.AddCharRange('A', 'Z');
            return list.ToArray();
        }
        
        private static char[] GetSecondChars()
        {
            var list = new List<char>();
            list.AddRange(GetAlphabeticChars());
            list.AddRange(GetSymbolChars());

            return list.ToArray();
        }

        private bool _isSymbolToken;

        public AideSyntaxTokenExtractor()
            : base(LexerHelper.StandardSpaceChars, FirstChars)
        {
        }

        protected override void Reset()
        {
            _isSymbolToken = false;
        }

        protected override IToken ProduceResult()
        {
            throw new NotImplementedException();
        }

        protected override TestCharResult TestCurrentChar()
        {
            var localPos = this.GetLocalPosition();
            var c = this.GetCurrentChar();

            if (localPos == 0)
            {
                return this.ContinueIf(c == FirstChars[0]); // redundant, but let it be...
            }
            else if (localPos == 1)
            {
                _isSymbolToken = SymbolChars.Contains(c);
                return this.ContinueIf(SecondChars.Contains(c));
            }

            if (this.IsSpaceChar(c))
            {
                // don't advance
                return TestCharResult.End;
            }

            throw new NotImplementedException();

            //if (_isSymbolToken)
            //{
            //    // there should be no "3rd" (index #2) char if 'is symbol token'
            //    return TestCharResult.NotAllowed;
            //}

            //return this.ContinueIf(AlphabeticChars.Contains(c));
        }
    }
}
