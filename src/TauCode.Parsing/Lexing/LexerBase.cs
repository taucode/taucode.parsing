using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.TextProcessing;
using TauCode.Parsing.TextProcessing.Processors;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Lexing
{
    public abstract class LexerBase : ILexer
    {
        #region Fields

        private LexingContext _context;

        private IList<ITextProcessor> _whiteSpaceSkippers;
        private IList<ITokenExtractor> _tokenExtractors;

        #endregion

        #region Private

        private bool SkipWhiteSpace()
        {
            var begin = _context.GetIndex();

            while (true)
            {
                if (_context.IsEnd())
                {
                    break;
                }

                var shifted = false;

                foreach (var whiteSpaceSkipper in this.WhiteSpaceSkippers)
                {
                    var c = _context.GetCurrentChar();
                    if (!whiteSpaceSkipper.AcceptsFirstChar(c))
                    {
                        continue;
                    }

                    var result = whiteSpaceSkipper.Process(_context);

                    if (result.IsSuccessful())
                    {
                        shifted = true;
                        _context.AdvanceByResult(result);
                        break;
                    }
                }

                if (!shifted)
                {
                    break;
                }
            }

            var end = _context.GetIndex();

            return end - begin > 0;
        }

        #endregion

        #region Abstract

        protected abstract IList<ITokenExtractor> CreateTokenExtractors();

        #endregion

        #region Protected

        protected virtual IList<ITextProcessor> CreateWhiteSpaceSkippers()
        {
            return new List<ITextProcessor>
            {
                new SpaceSkipper(),
                new NewLineSkipper(false),
            };
        }

        protected IList<ITextProcessor> WhiteSpaceSkippers
        {
            get
            {
                if (_whiteSpaceSkippers == null)
                {
                    _whiteSpaceSkippers = this.CreateWhiteSpaceSkippers();
                    if (_whiteSpaceSkippers == null)
                    {
                        throw LexingHelper.CreateInternalErrorLexingException(additionalInfo: $"'{nameof(CreateWhiteSpaceSkippers)}' returned null.");
                    }
                }

                return _whiteSpaceSkippers;
            }
        }

        protected IList<ITokenExtractor> TokenExtractors
        {
            get
            {
                if (_tokenExtractors == null)
                {
                    _tokenExtractors = this.CreateTokenExtractors();
                    if (_tokenExtractors == null || !_tokenExtractors.Any())
                    {
                        throw LexingHelper.CreateInternalErrorLexingException(additionalInfo: $"'{nameof(CreateTokenExtractors)}' returned null or empty collection.");
                    }
                }

                return _tokenExtractors;
            }
        }

        #endregion

        #region ILexer Members

        public IList<IToken> Lexize(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            _context = new LexingContext(input);
            var tokens = _context.GetTokenList();

            while (true)
            {
                // skippers begin to work
                var whiteSpaceSkipped = this.SkipWhiteSpace();
                if (_context.IsEnd())
                {
                    break;
                }

                // token extractors begin to work
                var gotSuccess = false;

                foreach (var tokenExtractor in this.TokenExtractors)
                {
                    if (gotSuccess)
                    {
                        break;
                    }

                    var c = _context.GetCurrentChar();
                    if (!tokenExtractor.AcceptsFirstChar(c))
                    {
                        continue;
                    }

                    var result = tokenExtractor.Process(_context);

                    if (result.IsSuccessful())
                    {
                        gotSuccess = true;
                        _context.Advance(result.IndexShift, result.LineShift, result.GetCurrentColumn());
                        var token = (IToken)result.Payload;

                        if (!(token is NullToken))
                        {
                            tokens.Add(token);
                        }
                    }
                }

                if (!whiteSpaceSkipped && !gotSuccess)
                {
                    var c = _context.GetCurrentChar();
                    throw new LexingException($"Unexpected char: '{c}'.", _context.GetCurrentPosition());
                }
            }

            return tokens;
        }

        #endregion
    }
}
