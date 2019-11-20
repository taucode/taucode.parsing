﻿using System;
using System.Collections.Generic;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.Parsing
{
    public class SqlLexer : ILexer
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
            '`',
            '~',
            '!',
            '@',
            '#',
            '$',
            '%',
            '^',
            '&',
            '*',
            '(',
            ')',
            ',',
            '.',
            ':',
            '/',
            '\\',
            '+',
            '-',
            '=',
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

        private char? TryGetNextChar()
        {
            if (this.IsEnd() || _pos + 1 == _input.Length)
            {
                return null;
            }

            return _input[_pos + 1];
        }

        private bool IsEnd() => _pos == _input.Length;

        private int GetCurrentPosition() => _pos;

        private void Advance(int step = 1)
        {
            if (this.IsEnd())
            {
                throw new NotImplementedException();
            }

            _pos += step;
        }

        private bool IsWhiteSpaceChar(char c)
        {
            return SpaceChars.Contains(c);
        }

        private bool IsSymbolChar(char c)
        {
            return SymbolChars.Contains(c);
        }

        private bool IsWordStartingChar(char c)
        {
            return
                c == '_' ||
                (c >= 'a' && c <= 'z') ||
                (c >= 'A' && c <= 'Z') ||
                false;
        }

        private bool IsWordContinuingChar(char c)
        {
            return
                this.IsWordStartingChar(c) ||
                (c >= '0' && c <= '9') ||
                false;
        }

        private bool IsDigitChar(char c)
        {
            return c >= '0' && c <= '9';
        }

        private bool IsIdentifierLeftDelimiterChar(char c)
        {
            return c == '[';
        }

        private bool IsIdentifierStartChar(char c)
        {
            return
                c == '_' ||
                (c >= 'a' && c <= 'z') ||
                (c >= 'A' && c <= 'Z') ||
                false;
        }

        private bool IsIdentifierContinuingChar(char c)
        {
            return
                this.IsIdentifierStartChar(c) ||
                (c >= '0' && c <= '9') ||
                false;
        }

        private WordToken ReadWordToken()
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

            if (word.Length == 0)
            {
                throw new NotImplementedException();
            }

            return new WordToken(word);
        }

        private SymbolToken ReadSymbolToken()
        {
            var c = this.GetCurrentChar();
            var next = this.TryGetNextChar();

            if (next.HasValue)
            {
                var nextChar = next.Value;
                if (
                    this.IsWhiteSpaceChar(nextChar) ||
                    this.IsWordStartingChar(nextChar) ||
                    this.IsDigitChar(nextChar) ||
                    this.IsSymbolChar(nextChar))
                {
                    this.Advance();
                    return new SymbolToken(c);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                this.Advance();
                return new SymbolToken(c);
            }
        }

        private IntegerToken ReadIntegerToken()
        {
            var start = this.GetCurrentPosition();

            while (true)
            {
                if (this.IsEnd())
                {
                    break;
                }

                var c = this.GetCurrentChar();
                if (this.IsDigitChar(c))
                {
                    this.Advance();
                    continue;
                }
                else if (this.IsSymbolChar(c) || this.IsWhiteSpaceChar(c))
                {
                    break;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            var end = this.GetCurrentPosition();
            var len = end - start;
            var word = _input.Substring(start, len);

            if (word.Length == 0)
            {
                throw new NotImplementedException();
            }

            return new IntegerToken(word);
        }

        private char ResolveRightIdentifierDelimiter(char leftDelimiter)
        {
            if (leftDelimiter == '[')
            {
                return ']';
            }

            throw new NotImplementedException();
        }

        private IdentifierToken ReadIdentifierToken(char? leftDelimiter)
        {
            char? rightDelimiter = null;
            if (leftDelimiter.HasValue)
            {
                rightDelimiter = this.ResolveRightIdentifierDelimiter(leftDelimiter.Value);
            }

            if (leftDelimiter.HasValue)
            {
                this.Advance();
            }

            var start = this.GetCurrentPosition();

            var identBegin = true;

            while (true)
            {
                if (this.IsEnd())
                {
                    break;
                }

                var c = this.GetCurrentChar();
                var isIdentChar =
                    (identBegin && this.IsIdentifierStartChar(c)) ||
                    (!identBegin && this.IsIdentifierContinuingChar(c));

                if (isIdentChar)
                {
                    this.Advance(); // go on
                }
                else if (c == rightDelimiter)
                {
                    this.Advance();
                    break;
                }
                else
                {
                    throw new NotImplementedException();
                }

                identBegin = false;
            }

            var end = this.GetCurrentPosition();
            var len = end - start;
            if (leftDelimiter.HasValue)
            {
                len--;
            }
            var identifier = _input.Substring(start, len);

            return new IdentifierToken(identifier);
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

                if (this.IsWhiteSpaceChar(c))
                {
                    this.Advance();
                }
                else if (this.IsWordStartingChar(c))
                {
                    var token = this.ReadWordToken();
                    list.Add(token);
                }
                else if (this.IsSymbolChar(c))
                {
                    var token = this.ReadSymbolToken();
                    list.Add(token);
                }
                else if (this.IsDigitChar(c))
                {
                    var token = this.ReadIntegerToken();
                    list.Add(token);
                }
                else if (this.IsIdentifierLeftDelimiterChar(c))
                {
                    var token = this.ReadIdentifierToken(c);
                    list.Add(token);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}
