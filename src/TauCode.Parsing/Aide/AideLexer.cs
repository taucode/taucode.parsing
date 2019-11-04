using System;
using System.Collections.Generic;
using TauCode.Parsing.Aide.Tokens;

namespace TauCode.Parsing.Aide
{
    public class AideLexer : ILexer
    {
        private static readonly HashSet<char> SpaceChars = new HashSet<char>(new[]
        {
            ' ',
            '\t',
            '\r',
            '\n'
        });

        private static readonly HashSet<char> SymbolChars = new HashSet<char>(new[]
        {
            '(',
            ')',
            ',',
        });

        private string _input;
        private int _pos;

        private char GetCurrentChar()
        {
            if (this.IsEnd())
            {
                throw new NotImplementedException();
            }

            return _input[_pos];
        }

        private int GetCurrentPosition() => _pos;

        private bool IsEnd() => _pos == _input.Length;

        private bool IsWhiteSpaceChar(char c)
        {
            return SpaceChars.Contains(c);
        }

        private bool IsWordStartingChar(char c)
        {
            // todo
            return
                c == '_' ||
                (c >= 'a' && c <= 'z') ||
                (c >= 'A' && c <= 'Z') ||
                false;
        }

        private bool IsWordContinuingChar(char c)
        {
            // todo
            return
                this.IsWordStartingChar(c) ||
                (c >= '0' && c <= '9') ||
                false;
        }

        private bool IsTokenNameChar(char c)
        {
            // todo
            return
                c == '_' ||
                (c >= 'a' && c <= 'z') ||
                (c >= 'A' && c <= 'Z') ||
                (c >= '0' && c <= '9') ||
                false;
        }

        private bool IsAliasedTokenChar(char c)
        {
            // todo
            return
                (c >= 'a' && c <= 'z') ||
                (c >= 'A' && c <= 'Z') ||
                false;
        }

        private bool IsSymbolChar(char c)
        {
            return SymbolChars.Contains(c);
        }

        private WordAideToken ReadWordToken(string tokenName)
        {
            var start = this.GetCurrentPosition();

            while (true)
            {
                if (this.IsEnd())
                {
                    break;
                }

                var c = this.GetCurrentChar();
                if (this.IsWordContinuingChar(c))
                {
                    this.Advance();
                    continue;
                }
                else
                {
                    break;
                }
            }

            var end = this.GetCurrentPosition();
            var len = end - start;
            var word = _input.Substring(start, len);

            // todo: check length
            return new WordAideToken(word, tokenName);
        }

        private AideToken ReadSpecialToken(string tokenName)
        {
            var start = this.GetCurrentPosition();

            while (true)
            {
                if (this.IsEnd())
                {
                    break;
                }

                var c = this.GetCurrentChar();

                if (this.IsSymbolChar(c))
                {
                    var delta = this.GetCurrentPosition() - start;
                    if (delta == 0)
                    {
                        this.Advance();
                        return new SymbolAideToken(c, tokenName);
                    }
                    else
                    {
                        throw new NotImplementedException(); // error
                    }
                }
                else if (this.IsAliasedTokenChar(c))
                {
                    this.Advance();
                    continue;
                }
                else if (this.IsWhiteSpaceChar(c))
                {
                    break;
                }
                else
                {
                    throw new NotImplementedException(); // error
                }
            }

            var end = this.GetCurrentPosition();
            var length = end - start;
            var alias = _input.Substring(start, length);
            AideToken aliasedToken;

            switch (alias)
            {
                case "Identifier":
                    aliasedToken = new IdentifierAideToken(tokenName);
                    break;

                case "Block":
                    aliasedToken = new BlockAideToken(tokenName);
                    break;

                case "End":
                    aliasedToken = new EndAideToken(tokenName);
                    break;

                default:
                    throw new NotImplementedException(); // error
            }

            return aliasedToken;
        }

        private string ReadTokenName()
        {
            var start = this.GetCurrentPosition();

            while (true)
            {
                if (this.IsEnd())
                {
                    throw new NotImplementedException(); // error
                }

                var c = this.GetCurrentChar();
                if (this.IsTokenNameChar(c))
                {
                    this.Advance();
                    continue;
                }
                else if (c == '>')
                {
                    this.Advance();
                    break;
                }
                else
                {
                    throw new NotImplementedException(); // error
                }
            }

            var end = this.GetCurrentPosition();
            var length = end - start - 1;
            var tokenName = _input.Substring(start, length);

            // todo: check length.
            return tokenName;
        }

        private void Advance()
        {
            if (this.IsEnd())
            {
                throw new NotImplementedException();
            }

            _pos++;
        }

        public List<IToken> Lexize(string input)
        {
            _input = input ?? throw new ArgumentNullException(nameof(input));
            _pos = 0;
            var list = new List<IToken>();
            string upcomingTokenName = null;

            while (true)
            {
                if (this.IsEnd())
                {
                    return list;
                }

                var c = this.GetCurrentChar();

                if (this.IsWhiteSpaceChar(c))
                {
                    if (upcomingTokenName != null)
                    {
                        throw new NotImplementedException(); // error
                    }

                    this.Advance();
                }
                else if (this.IsWordStartingChar(c))
                {
                    var token = this.ReadWordToken(upcomingTokenName);
                    upcomingTokenName = null;

                    list.Add(token);
                }
                else if (c == '\\')
                {
                    this.Advance();

                    var token = this.ReadSpecialToken(upcomingTokenName);
                    upcomingTokenName = null;

                    list.Add(token);
                }
                else if (c == '<')
                {
                    if (upcomingTokenName != null)
                    {
                        throw new NotImplementedException(); // error
                    }

                    this.Advance();
                    upcomingTokenName = this.ReadTokenName();
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}
