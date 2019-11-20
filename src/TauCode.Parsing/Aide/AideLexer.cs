using System;
using System.Collections.Generic;
using TauCode.Parsing.Tokens;

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
                throw LexerHelper.CreateUnexpectedEndOfInputException();
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

        private bool IsTokenNameChar(char c)
        {
            return
                c == '_' ||
                (c >= 'a' && c <= 'z') ||
                (c >= 'A' && c <= 'Z') ||
                (c >= '0' && c <= '9') ||
                false;
        }

        private bool IsAliasedTokenChar(char c)
        {
            return
                (c >= 'a' && c <= 'z') ||
                (c >= 'A' && c <= 'Z') ||
                false;
        }

        private bool IsSymbolChar(char c)
        {
            return SymbolChars.Contains(c);
        }

        private WordToken ReadWordToken(string tokenName)
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
                throw LexerHelper.CreateEmptyTokenException();
            }

            return new WordToken(word, tokenName);
        }

        private TokenBase ReadSpecialToken(string tokenName)
        {
            if (this.IsEnd())
            {
                throw LexerHelper.CreateUnexpectedEndOfInputException();
            }

            var start = this.GetCurrentPosition();

            var c = this.GetCurrentChar();

            if (this.IsSymbolChar(c))
            {
                this.Advance();
                return new SymbolToken(c, tokenName);
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
                    throw new AideException($"Unexpected char: '{c}'.");
                }
            }

            var end = this.GetCurrentPosition();
            var length = end - start;

            if (length == 0)
            {
                throw LexerHelper.CreateEmptyTokenException();
            }

            var alias = _input.Substring(start, length);
            TokenBase aliasedToken;

            switch (alias)
            {
                case "BeginBlockDefinition":
                    aliasedToken = new EnumToken<SyntaxElement>(SyntaxElement.BeginBlockDefinition, tokenName);
                    break;

                case "EndBlockDefinition":
                    aliasedToken = new EnumToken<SyntaxElement>(SyntaxElement.EndBlockDefinition, tokenName);
                    break;

                case "Identifier":
                    aliasedToken = new EnumToken<SyntaxElement>(SyntaxElement.Identifier, tokenName);
                    break;

                case "BlockReference":
                    aliasedToken = new EnumToken<SyntaxElement>(SyntaxElement.BlockReference, tokenName);
                    break;

                case "Idle":
                    aliasedToken = new EnumToken<SyntaxElement>(SyntaxElement.Idle, tokenName);
                    break;

                case "Word":
                    aliasedToken = new EnumToken<SyntaxElement>(SyntaxElement.Word, tokenName);
                    break;

                case "Integer":
                    aliasedToken = new EnumToken<SyntaxElement>(SyntaxElement.Integer, tokenName);
                    break;

                case "End":
                    aliasedToken = new EnumToken<SyntaxElement>(SyntaxElement.End, tokenName);
                    break;

                default:
                    throw new AideException($"Unknown alias: '{alias}'.");
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
                    throw LexerHelper.CreateUnexpectedEndOfInputException();
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
                    throw LexerHelper.CreateUnexpectedCharException(c);
                }
            }

            var end = this.GetCurrentPosition();
            var length = end - start - 1;

            if (length == 0)
            {
                throw LexerHelper.CreateEmptyTokenException();
            }

            var tokenName = _input.Substring(start, length);
            return tokenName;
        }

        private SpecialStringToken<AideSpecialString> ReadNameReference()
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
            var length = end - start;

            if (length == 0)
            {
                throw LexerHelper.CreateEmptyTokenException();
            }

            var referencedName = _input.Substring(start, length);
            return new SpecialStringToken<AideSpecialString>(AideSpecialString.NameReference, referencedName);
        }

        private void SkipComment()
        {
            // comment must start with '/*'
            var c = this.GetCurrentChar();
            if (c != '/')
            {
                throw LexerHelper.CreateUnexpectedCharException(c);
            }

            this.Advance();
            c = this.GetCurrentChar();
            if (this.GetCurrentChar() != '*')
            {
                throw LexerHelper.CreateUnexpectedCharException(c);
            }

            this.Advance();

            while (true)
            {
                if (this.IsEnd())
                {
                    throw LexerHelper.CreateUnexpectedEndOfInputException();
                }

                c = this.GetCurrentChar();

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
                throw LexerHelper.CreateUnexpectedEndOfInputException();
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
                        throw AideHelper.CreateTokenNameCannotPrecedeChar(c);
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
                        throw AideHelper.CreateTokenNameCannotPrecedeChar(c);
                    }

                    this.Advance();
                    upcomingTokenName = this.ReadTokenName();
                }
                else if (c == '/')
                {
                    var nextChar = this.TryGetNextChar();
                    if (nextChar.HasValue)
                    {
                        var nextCharValue = nextChar.Value;
                        if (nextCharValue == '*')
                        {
                            if (upcomingTokenName != null)
                            {
                                throw AideHelper.CreateTokenNameCannotPrecedeChar(c);
                            }

                            this.SkipComment();
                        }
                        else
                        {
                            throw LexerHelper.CreateUnexpectedCharException(nextCharValue);
                        }
                    }
                    else
                    {
                        throw LexerHelper.CreateUnexpectedEndOfInputException();
                    }
                }
                else if (c == '(')
                {
                    if (upcomingTokenName != null)
                    {
                        throw AideHelper.CreateTokenNameCannotPrecedeChar(c);
                    }

                    this.Advance();
                    var token = new EnumToken<SyntaxElement>(SyntaxElement.LeftParenthesis, null);
                    list.Add(token);
                }
                else if (c == ')')
                {
                    if (upcomingTokenName != null)
                    {
                        throw AideHelper.CreateTokenNameCannotPrecedeChar(c);
                    }

                    this.Advance();
                    var token = new EnumToken<SyntaxElement>(SyntaxElement.RightParenthesis, null);
                    list.Add(token);
                }
                else if (c == ':')
                {
                    if (upcomingTokenName != null)
                    {
                        throw AideHelper.CreateTokenNameCannotPrecedeChar(c);
                    }

                    this.Advance();
                    var token = this.ReadNameReference();
                    list.Add(token);
                }
                else if (c == '[')
                {
                    this.Advance();
                    var token = new EnumToken<SyntaxElement>(SyntaxElement.LeftBracket, upcomingTokenName);
                    upcomingTokenName = null;

                    list.Add(token);
                }
                else if (c == ']')
                {
                    if (upcomingTokenName != null)
                    {
                        throw AideHelper.CreateTokenNameCannotPrecedeChar(c);
                    }

                    this.Advance();
                    var token = new EnumToken<SyntaxElement>(SyntaxElement.RightBracket, null);
                    list.Add(token);
                }
                else if (c == '{')
                {
                    this.Advance();
                    var token = new EnumToken<SyntaxElement>(SyntaxElement.LeftCurlyBracket, upcomingTokenName);
                    upcomingTokenName = null;

                    list.Add(token);
                }
                else if (c == '}')
                {
                    if (upcomingTokenName != null)
                    {
                        throw AideHelper.CreateTokenNameCannotPrecedeChar(c);
                    }

                    this.Advance();
                    var token = new EnumToken<SyntaxElement>(SyntaxElement.RightCurlyBracket, null);
                    list.Add(token);
                }
                else if (c == '|')
                {
                    if (upcomingTokenName != null)
                    {
                        throw AideHelper.CreateTokenNameCannotPrecedeChar(c);
                    }

                    this.Advance();
                    var token = new EnumToken<SyntaxElement>(SyntaxElement.VerticalBar, null);
                    list.Add(token);
                }
                else if (c == ',')
                {
                    if (upcomingTokenName != null)
                    {
                        throw AideHelper.CreateTokenNameCannotPrecedeChar(c);
                    }

                    this.Advance();
                    var token = new EnumToken<SyntaxElement>(SyntaxElement.Comma, null);
                    list.Add(token);
                }
                else
                {
                    throw LexerHelper.CreateUnexpectedCharException(c);
                }
            }
        }
    }
}
