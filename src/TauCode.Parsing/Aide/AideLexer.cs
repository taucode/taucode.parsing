﻿using System;
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

        private char? TryGetNextChar()
        {
            if (this.IsEnd())
            {
                return null;
            }

            return _input[_pos + 1];
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
            if (this.IsEnd())
            {
                throw new NotImplementedException(); // error
            }

            var start = this.GetCurrentPosition();

            var c = this.GetCurrentChar();

            if (this.IsSymbolChar(c))
            {
                this.Advance();
                return new SymbolAideToken(c, tokenName);
            }

            while (true)
            {
                if (this.IsEnd())
                {
                    break;
                }

                c = this.GetCurrentChar();

                if (this.IsWhiteSpaceChar(c))
                {
                    break;
                }
                if (this.IsAliasedTokenChar(c))
                {
                    this.Advance();
                    continue;
                }
                else if (this.IsSymbolChar(c))
                {
                    break;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            var end = this.GetCurrentPosition();
            var length = end - start;

            if (length == 0)
            {
                throw new NotImplementedException(); // error
            }

            var alias = _input.Substring(start, length);
            AideToken aliasedToken;

            switch (alias)
            {
                case "BeginBlock":
                    aliasedToken = new SyntaxElementAideToken(SyntaxElement.BeginBlock, tokenName);
                    break;

                case "EndBlock":
                    aliasedToken = new SyntaxElementAideToken(SyntaxElement.EndBlock, tokenName);
                    break;

                case "CloneBlock":
                    aliasedToken = new SyntaxElementAideToken(SyntaxElement.CloneBlock, tokenName);
                    break;

                case "Identifier":
                    aliasedToken = new SyntaxElementAideToken(SyntaxElement.Identifier, tokenName);
                    break;

                case "Block":
                    aliasedToken = new SyntaxElementAideToken(SyntaxElement.Block, tokenName);
                    break;

                case "End":
                    aliasedToken = new SyntaxElementAideToken(SyntaxElement.End, tokenName);
                    break;

                case "Link":
                    aliasedToken = new SyntaxElementAideToken(SyntaxElement.Link, tokenName);
                    break;

                case "Idle":
                    aliasedToken = new SyntaxElementAideToken(SyntaxElement.Idle, tokenName);
                    break;

                case "WrongWay":
                    aliasedToken = new SyntaxElementAideToken(SyntaxElement.WrongWay, tokenName);
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

        private NameReferenceAideToken ReadTokenNameReference()
        {
            var start = this.GetCurrentPosition();

            while (true)
            {
                if (this.IsEnd())
                {
                    break;
                }

                var c = this.GetCurrentChar();

                if (this.IsTokenNameChar(c))
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
            var length = end - start - 1;
            var referencedTokenName = _input.Substring(start, length);

            // todo: check length.
            return new NameReferenceAideToken(referencedTokenName);

        }

        private void SkipComment()
        {
            // comment must start with '/*'
            if (this.GetCurrentChar() != '/')
            {
                throw new NotImplementedException(); // todo
            }

            this.Advance();
            if (this.GetCurrentChar() != '*')
            {
                throw new NotImplementedException(); // todo
            }

            this.Advance();

            while (true)
            {
                if (this.IsEnd())
                {
                    throw new NotImplementedException(); // not closed comment
                }

                var c = this.GetCurrentChar();

                if (c == '*')
                {
                    var next = this.TryGetNextChar();
                    if (next == '/')
                    {
                        // comment is closed
                        this.Advance(2);
                        return;
                    }
                }

                this.Advance();
            }
        }

        private void Advance(int step = 1)
        {
            if (this.IsEnd())
            {
                throw new NotImplementedException();
            }

            _pos += step;
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
                else if (c == '/')
                {
                    if (this.TryGetNextChar() == '*')
                    {
                        if (upcomingTokenName != null)
                        {
                            throw new NotImplementedException(); // error
                        }

                        this.SkipComment();
                    }
                    else
                    {
                        throw new NotImplementedException(); // error
                    }
                }
                else if (c == '(')
                {
                    if (upcomingTokenName != null)
                    {
                        throw new NotImplementedException(); // error
                    }

                    this.Advance();
                    var token = new SyntaxElementAideToken(SyntaxElement.LeftParenthesis, null);
                    list.Add(token);
                }
                else if (c == ')')
                {
                    if (upcomingTokenName != null)
                    {
                        throw new NotImplementedException(); // error
                    }

                    this.Advance();
                    var token = new SyntaxElementAideToken(SyntaxElement.RightParenthesis, null);
                    list.Add(token);
                }
                else if (c == ':')
                {
                    if (upcomingTokenName != null)
                    {
                        throw new NotImplementedException(); // error
                    }

                    this.Advance();
                    var token = this.ReadTokenNameReference();
                    list.Add(token);
                }
                else if (c == '[')
                {
                    if (upcomingTokenName != null)
                    {
                        throw new NotImplementedException(); // error
                    }

                    this.Advance();
                    var token = new SyntaxElementAideToken(SyntaxElement.LeftBracket, null);
                    list.Add(token);
                }
                else if (c == ']')
                {
                    if (upcomingTokenName != null)
                    {
                        throw new NotImplementedException(); // error
                    }

                    this.Advance();
                    var token = new SyntaxElementAideToken(SyntaxElement.RightBracket, null);
                    list.Add(token);
                }
                else if (c == '{')
                {
                    if (upcomingTokenName != null)
                    {
                        throw new NotImplementedException(); // error
                    }

                    this.Advance();
                    var token = new SyntaxElementAideToken(SyntaxElement.LeftCurlyBracket, null);
                    list.Add(token);
                }
                else if (c == '}')
                {
                    if (upcomingTokenName != null)
                    {
                        throw new NotImplementedException(); // error
                    }

                    this.Advance();
                    var token = new SyntaxElementAideToken(SyntaxElement.RightCurlyBracket, null);
                    list.Add(token);
                }
                else if (c == '|')
                {
                    if (upcomingTokenName != null)
                    {
                        throw new NotImplementedException(); // error
                    }

                    this.Advance();
                    var token = new SyntaxElementAideToken(SyntaxElement.VerticalBar, null);
                    list.Add(token);
                }
                else if (c == ',')
                {
                    if (upcomingTokenName != null)
                    {
                        throw new NotImplementedException(); // error
                    }

                    this.Advance();
                    var token = new SyntaxElementAideToken(SyntaxElement.Comma, null);
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
