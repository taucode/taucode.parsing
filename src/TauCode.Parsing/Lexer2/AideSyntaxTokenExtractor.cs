using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Aide;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Lexer2
{
    public class AideSyntaxTokenExtractor : TokenExtractorBase
    {
        private static readonly char[] FirstChars = { '\\', '(', ')', '[', ']', '{', '}', '|', ',' };
        //private static readonly HashSet<char> SymbolChars = new HashSet<char>(GetSymbolChars());
        private static readonly HashSet<char> AlphabeticChars = new HashSet<char>(GetAlphabeticChars());
        private static readonly HashSet<char> AideSymbols = new HashSet<char>(new[] { '(', ')', '[', ']', '{', '}', '|', ',' });

        //private static readonly HashSet<char> SecondChars = new HashSet<char>(GetSecondChars());

        private static char[] GetAlphabeticChars()
        {
            var list = new List<char>();
            list.AddCharRange('a', 'z');
            list.AddCharRange('A', 'Z');
            return list.ToArray();
        }

        //private static char[] GetSecondChars()
        //{
        //    var list = new List<char>();
        //    list.AddRange(GetAlphabeticChars());
        //    list.AddRange(GetSymbolChars());

        //    return list.ToArray();
        //}

        //private bool _isSymbolToken;

        public AideSyntaxTokenExtractor()
            : base(LexerHelper.StandardSpaceChars, FirstChars)
        {
        }

        protected override void Reset()
        {
            //_isSymbolToken = false;
        }

        protected override IToken ProduceResult()
        {
            SyntaxElement syntaxElement;
            var resultString = this.ExtractResultString();
            if (resultString.Length == 1)
            {
                switch (resultString.Single())
                {
                    case '(':
                        syntaxElement = SyntaxElement.LeftParenthesis;
                        break;

                    case ')':
                        syntaxElement = SyntaxElement.RightParenthesis;
                        break;

                    case '{':
                        syntaxElement = SyntaxElement.LeftCurlyBracket;
                        break;

                    case '}':
                        syntaxElement = SyntaxElement.RightCurlyBracket;
                        break;

                    case ',':
                        syntaxElement = SyntaxElement.Comma;
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }
            else
            {
                var str = this.ExtractResultString().Substring(1);

                var parsed = Enum.TryParse(str, out syntaxElement);

                if (!parsed)
                {
                    return null;
                }

                //if (parsed)
                //{
                //    return new EnumToken<SyntaxElement>(syntaxElement);
                //}

                //return null;
            }

            return new EnumToken<SyntaxElement>(syntaxElement);


            //var str = this.ExtractResultString().Substring(1);

            //var parsed = Enum.TryParse(str, out SyntaxElement syntaxElement);
            //if (parsed)
            //{
            //    return new EnumToken<SyntaxElement>(syntaxElement);
            //}

            //return null;
        }

        protected override TestCharResult TestCurrentChar()
        {
            var localPos = this.GetLocalPosition();
            var c = this.GetCurrentChar();

            if (localPos == 0)
            {
                if (AideSymbols.Contains(c)) // got single-char syntax element/symbol.
                {
                    this.Advance();
                    return TestCharResult.End;
                }

                return TestCharResult.Continue;
            }

            if (AlphabeticChars.Contains(c))
            {
                return TestCharResult.Continue;
            }

            return TestCharResult.End;

            //else if (localPos == 1)
            //{
            //    throw new NotImplementedException();
            //    //_isSymbolToken = SymbolChars.Contains(c);
            //    //return this.ContinueIf(SecondChars.Contains(c));
            //}

            //throw new NotImplementedException();

            //if (this.IsSpaceChar(c))
            //{
            //    // don't advance
            //    return TestCharResult.End;
            //}

            //throw new NotImplementedException();

            //if (_isSymbolToken)
            //{
            //    // there should be no "3rd" (index #2) char if 'is symbol token'
            //    return TestCharResult.NotAllowed;
            //}

            //return this.ContinueIf(AlphabeticChars.Contains(c));
        }
    }
}
