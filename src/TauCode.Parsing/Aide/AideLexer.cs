using System;
using System.Collections.Generic;

namespace TauCode.Parsing.Aide
{
    public class AideLexer : ILexer
    {
        private string _input;
        private int _pos;

        private char GetCurrentChar()
        {
            throw new NotImplementedException();
        }

        private bool IsEnd()
        {
            throw new NotImplementedException();
        }

        private bool IsWhiteSpace(char c)
        {
            throw new NotImplementedException();
        }
        private void Advance()
        {
            throw new NotImplementedException();
        }

        public List<IToken> Lexize(string input)
        {
            _input = input ?? throw new ArgumentNullException(nameof(input));
            _pos = 0;
            var list = new List<IToken>();

            while (true)
            {
                if (this.IsEnd())
                {
                    return list;
                }

                var c = this.GetCurrentChar();
                if (this.IsWhiteSpace(c))
                {
                    this.Advance();
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}
