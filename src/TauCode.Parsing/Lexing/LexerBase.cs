using System;
using System.Collections.Generic;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.TextProcessing;
using TauCode.Parsing.TextProcessing.Processors;

namespace TauCode.Parsing.Lexing
{
    public abstract class LexerBase : ILexer
    {
        #region Fields

        private LexingContext _context;
        private IList<ITokenExtractor> _tokenExtractors;
        private IList<ITextProcessor<string>> _skippers;

        #endregion

        #region Private

        private bool SkipWhilePossible()
        {
            var eventuallySkipped = false;

            while (true)
            {
                if (_context.IsEnd())
                {
                    break;
                }

                var skipped = false;

                foreach (var skipper in this.Skippers)
                {
                    var c = _context.GetCurrentChar();
                    if (!skipper.AcceptsFirstChar(c))
                    {
                        continue;
                    }

                    if (skipper.IsProcessing)
                    {
                        throw new NotImplementedException();
                    }

                    var skipResult = skipper.Process(_context);

                    if (skipper.IsProcessing)
                    {
                        throw new NotImplementedException();
                    }

                    if (_context.Depth != 1)
                    {
                        throw new LexingException($"Depth of '{typeof(ILexingContext).FullName}.{nameof(ILexingContext.Depth)}' must be equal to 1 when exiting skipper.", _context.GetCurrentAbsolutePosition());
                    }

                    if (skipResult.Summary == TextProcessingSummary.Skip)
                    {
                        skipped = true;
                        eventuallySkipped = true;
                        _context.Advance(skipResult.IndexShift, skipResult.LineShift, skipResult.GetCurrentColumn());
                        break;
                    }
                    else if (skipResult.Summary == TextProcessingSummary.CanProduce)
                    {
                        throw new NotImplementedException(); // should never happen, skippers only 'skip' or 'fail'.
                    }
                }

                if (!skipped)
                {
                    break;
                }
            }

            return eventuallySkipped;
        }

        #endregion

        #region Abstract

        protected abstract IList<ITokenExtractor> CreateTokenExtractors();

        #endregion

        #region Protected

        protected IList<ITokenExtractor> TokenExtractors =>
            _tokenExtractors ?? (_tokenExtractors = this.CreateTokenExtractors());

        protected IList<ITextProcessor<string>> Skippers => _skippers ?? (_skippers = this.CreateSkippers());

        protected virtual IList<ITextProcessor<string>> CreateSkippers()
        {
            return new ITextProcessor<string>[]
            {
                new SkipSpacesProcessor(),
                new SkipLineBreaksProcessor(false),
            };
        }

        #endregion

        #region ILexer Members

        public IList<IToken> Lexize(string input)
        {
            // todo check args
            //var tokens = new List<IToken>();
            //_context = new TextProcessingContext(input);

            _context = new LexingContext(input);


            var tokens = _context.GetTokenList();

            while (true)
            {
                // skippers begin to work
                var skipped = this.SkipWhilePossible();
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

                    var previousColumn = _context.GetCurrentColumn();

                    if (tokenExtractor.IsProcessing)
                    {
                        throw new NotImplementedException();
                    }
                    var result = tokenExtractor.Process(_context);
                    if (tokenExtractor.IsProcessing)
                    {
                        throw new NotImplementedException();
                    }


                    if (_context.Depth != 1)
                    {
                        throw new NotImplementedException(); // todo error
                    }

                    switch (result.Summary)
                    {
                        case TextProcessingSummary.Skip:
                            gotSuccess = true;
                            _context.Advance(result.IndexShift, result.LineShift, result.GetCurrentColumn());
                            break;

                        case TextProcessingSummary.CanProduce:
                            var absoluteIndex = _context.GetAbsoluteIndex();
                            var consumedLength = result.IndexShift;

                            var line = _context.GetCurrentLine() + result.LineShift;
                            var position = new Position(line, previousColumn);

                            var token = tokenExtractor.Produce(
                                _context.Text,
                                absoluteIndex,
                                position,
                                consumedLength);

                            if (token == null)
                            {
                                // No luck. It can happen - for example, Lisp Symbol extractor will accept every char of "1111", but will refuse produce a whole result,
                                // because "1111" should be lexized as an Integer, not a Symbol.
                            }
                            else
                            {
                                gotSuccess = true;
                                _context.Advance(result.IndexShift, result.LineShift, result.GetCurrentColumn());

                                if (token.HasPayload)
                                {
                                    tokens.Add(token);
                                }
                            }

                            break;

                        case TextProcessingSummary.Fail: // no luck for this extractor
                            break;

                        default:
                            throw new NotImplementedException();
                    }
                }

                if (!skipped && !gotSuccess)
                {
                    var c = _context.GetCurrentChar();
                    throw new LexingException($"Unexpected char: '{c}'.", _context.GetCurrentAbsolutePosition());
                }
            }

            return tokens;
        }

        #endregion
    }
}
