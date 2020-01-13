using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Extensions;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.TextProcessing;

namespace TauCode.Parsing.Lexing
{
    public abstract class TokenExtractorBase<TToken> : TextProcessorBase, ITokenExtractor
        where TToken : IToken
    {
        #region Fields

        private readonly HashSet<Type> _acceptablePreviousTokenTypes;

        #endregion

        #region Constructor

        protected TokenExtractorBase(Type[] acceptablePreviousTokenTypes)
        {
            acceptablePreviousTokenTypes = (acceptablePreviousTokenTypes ?? Type.EmptyTypes).ToArray();
            if (acceptablePreviousTokenTypes.Any(x =>
                x == null ||
                !x.GetInterfaces().Contains(typeof(IToken))))
            {
                throw new ArgumentException("Invalid token types.", nameof(acceptablePreviousTokenTypes));
            }

            _acceptablePreviousTokenTypes = new HashSet<Type>(acceptablePreviousTokenTypes);
        }

        #endregion

        #region Abstract

        protected abstract CharAcceptanceResult AcceptCharImpl(char c, int localIndex);

        protected abstract void OnBeforeProcess();

        protected abstract TToken DeliverToken(string text, int absoluteIndex, Position position, int consumedLength);

        #endregion

        #region Protected

        protected virtual void ConsumeSubPayload(IPayload subPayload)
        {
            throw new LexingException("Override if you consume sub-payloads", null);
        }

        protected virtual bool AcceptsPreviousTokenImpl(IToken previousToken) =>
            _acceptablePreviousTokenTypes.Contains(previousToken.GetType());

        protected virtual void OnCharAccepted(char c)
        {
            // idle
        }

        protected ILexingContext Context { get; private set; }

        protected virtual bool ProcessEnd()
        {
            // idle, no problem for most token extractors.
            //
            // but of course will fail for unclosed strings, SQL identifiers [my_column_name , etc.
            // in such a case, throw an 'Unexpected-end' LexingException here.

            return true;
        }

        protected Position StartPosition { get; private set; }

        protected CharAcceptanceResult ContinueOrFail(bool b)
        {
            return b ? CharAcceptanceResult.Continue : CharAcceptanceResult.Fail;
        }

        protected virtual TextProcessingResult SubProcess() => TextProcessingResult.Failure;

        #endregion

        #region Overridden

        public override bool AcceptsFirstChar(char c)
        {
            var charAcceptanceResult = this.AcceptCharImpl(c, 0);
            switch (charAcceptanceResult)
            {
                case CharAcceptanceResult.Continue:
                    return true;

                case CharAcceptanceResult.Stop:
                    throw LexingHelper.CreateErrorInLogicLexingException();

                case CharAcceptanceResult.Fail:
                    return false;

                default:
                    throw LexingHelper.CreateErrorInLogicLexingException();
            }
        }

        public override TextProcessingResult Process(ITextProcessingContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var lexingContext = (ILexingContext)context;

            var previousChar = lexingContext.TryGetPreviousChar();
            if (previousChar.HasValue && !LexingHelper.IsInlineWhiteSpaceOrCaretControl(previousChar.Value))
            {
                var previousToken = lexingContext.GetLastToken();
                if (previousToken != null)
                {
                    var acceptsPreviousToken = this.AcceptsPreviousTokenImpl(previousToken);

                    if (!acceptsPreviousToken)
                    {
                        return TextProcessingResult.Failure;
                    }
                }
            }

            this.Context = lexingContext;
            this.StartPosition = this.Context.GetCurrentPosition();
            this.Context.RequestGeneration();

            // since 'Process' has been called, it means that 'First' (i.e. 0th) char was accepted by Lexer.
            this.Context.AdvanceByChar();

            this.OnBeforeProcess();

            var gotStop = false;

            while (true)
            {
                var isEnd = this.Context.IsEnd();

                if (isEnd || gotStop)
                {
                    if (this.Context.IndexOffset == 0)
                    {
                        throw LexingHelper.CreateErrorInLogicLexingException();
                    }

                    if (isEnd && !gotStop)
                    {
                        var acceptsEnd = this.ProcessEnd(); // throw here if you need.
                        if (!acceptsEnd)
                        {
                            this.Context.ReleaseGeneration();
                            this.Context = null;
                            return TextProcessingResult.Failure;
                        }
                    }

                    var myAbsoluteIndex = this.Context.GetIndex();
                    var myLine = this.Context.Line;
                    var currentColumn = this.Context.Column;

                    this.Context.ReleaseGeneration();

                    var oldAbsoluteIndex = this.Context.GetIndex();
                    var oldLine = this.Context.Line;
                    var oldColumn = this.Context.Column;

                    var indexShift = myAbsoluteIndex - oldAbsoluteIndex;
                    var lineShift = myLine - oldLine;


                    var position = new Position(oldLine, oldColumn);
                    var token = this.DeliverToken(this.Context.Text, oldAbsoluteIndex, position, indexShift);

                    this.Context = null;

                    return token == null
                        ? TextProcessingResult.Failure
                        : new TextProcessingResult(indexShift, lineShift, currentColumn, token);
                }

                // dealing with sub-process
                while (true)
                {
                    var subProcessResult = this.SubProcess();
                    if (subProcessResult.IsSuccessful())
                    {
                        this.ConsumeSubPayload(subProcessResult.Payload);
                        this.Context.AdvanceByResult(subProcessResult);
                    }
                    else
                    {
                        break;
                    }
                }

                // deal with char by my own.
                var c = this.Context.GetCurrentChar();
                var localIndex = this.Context.IndexOffset;

                var oldContextVersion = this.Context.Version;
                var acceptanceResult = this.AcceptCharImpl(c, localIndex);

                // check.
                if (this.Context.IndexOffset == 0 &&
                    !acceptanceResult.IsIn(CharAcceptanceResult.Continue, CharAcceptanceResult.Fail))
                {
                    throw LexingHelper.CreateErrorInLogicLexingException();
                }

                // check: only 'Stop' allows altering of context's version.
                if (acceptanceResult != CharAcceptanceResult.Stop)
                {
                    var newContextVersion = this.Context.Version;
                    if (oldContextVersion != newContextVersion)
                    {
                        throw LexingHelper.CreateErrorInLogicLexingException();
                    }
                }

                switch (acceptanceResult)
                {
                    case CharAcceptanceResult.Continue:
                        this.OnCharAccepted(c);
                        this.Context.AdvanceByChar();
                        break;

                    case CharAcceptanceResult.Stop:
                        gotStop = true;
                        break;

                    case CharAcceptanceResult.Fail:
                        this.Context.ReleaseGeneration();
                        this.Context = null;
                        return TextProcessingResult.Failure;

                    default:
                        throw LexingHelper.CreateErrorInLogicLexingException();
                }
            }
        }

        #endregion
    }
}
