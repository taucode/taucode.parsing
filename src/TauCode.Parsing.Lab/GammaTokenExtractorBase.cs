using System;
using TauCode.Extensions;

namespace TauCode.Parsing.Lab
{
    // todo clean up
    public abstract class GammaTokenExtractorBase<TToken> : IGammaTokenExtractor<TToken>
        where TToken : IToken
    {
        protected enum CharAcceptanceResult
        {
            Continue = 1,
            Stop,
            Fail,
        }

        protected TextProcessingContext Context { get; private set; }

        protected virtual TextProcessingResult Delegate()
        {
            return TextProcessingResult.Fail;
        }

        public abstract TToken ProduceToken(string text, int absoluteIndex, int consumedLength, Position position);

        public TextProcessingResult Process(TextProcessingContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var previousChar = context.GetPreviousAbsoluteChar();
            if (previousChar.HasValue)
            {
                var acceptsChar = this.AcceptsPreviousCharImpl(previousChar.Value);
                if (!acceptsChar)
                {
                    return TextProcessingResult.Fail;
                }
            }

            this.Context = context;
            this.Context.RequestGeneration();

            var stop = false;

            while (true)
            {
                var isEnd = this.Context.IsEnd();

                if (isEnd || stop)
                {
                    if (this.Context.GetLocalIndex() == 0)
                    {
                        throw new NotImplementedException(); // todo: how on earth did we get here?
                    }

                    if (isEnd)
                    {
                        var acceptsEnd = this.ProcessEnd(); // throw here if you need.
                        if (!acceptsEnd)
                        {
                            this.Context.ReleaseGeneration();
                            return TextProcessingResult.Fail;
                        }
                    }

                    var summary = this.IsProducer ? TextProcessingSummary.CanProduce : TextProcessingSummary.Skip;

                    var myAbsoluteIndex = this.Context.GetAbsoluteIndex();
                    var myLine = this.Context.GetCurrentLine();
                    var currentColumn = this.Context.GetCurrentColumn();

                    this.Context.ReleaseGeneration();

                    var oldAbsoluteIndex = this.Context.GetAbsoluteIndex();
                    var oldLine = this.Context.GetCurrentLine();

                    var indexShift = myAbsoluteIndex - oldAbsoluteIndex;
                    var lineShift = myLine - oldLine;
                    
                    return new TextProcessingResult(summary, indexShift, lineShift, currentColumn);
                }

                var delegatedResult = this.Delegate();
                if (delegatedResult.Summary != TextProcessingSummary.Fail)
                {
                    throw new NotImplementedException();
                }

                var c = this.Context.GetCurrentChar();
                var localIndex = this.Context.GetLocalIndex();

                var oldContextVersion = this.Context.Version;
                var acceptanceResult = this.AcceptCharImpl(c, localIndex);

                // check.
                if (this.Context.GetLocalIndex() == 0 && !acceptanceResult.IsIn(CharAcceptanceResult.Continue, CharAcceptanceResult.Fail))
                {
                    throw new NotImplementedException(); // todo error in your logic.
                }

                // check: only 'Stop' allows altering of context's version.
                if (acceptanceResult != CharAcceptanceResult.Stop)
                {
                    var newContextVersion = this.Context.Version;
                    if (oldContextVersion != newContextVersion)
                    {
                        throw new NotImplementedException();
                    }
                }

                switch (acceptanceResult)
                {
                    case CharAcceptanceResult.Continue:
                        this.Context.AdvanceByChar();
                        break;

                    case CharAcceptanceResult.Stop:
                        stop = true;
                        break;

                    case CharAcceptanceResult.Fail:
                        this.Context.ReleaseGeneration();
                        return TextProcessingResult.Fail;
                        
                    //break;

                    default:
                        throw new NotImplementedException(); // wtf? (todo)
                }
            }
        }

        public IToken Produce(string text, int absoluteIndex, int consumedLength, Position position)
            => this.ProduceToken(text, absoluteIndex, consumedLength, position);

        public bool AcceptsFirstChar(char c)
        {
            var charAcceptanceResult = this.AcceptCharImpl(c, 0);
            switch (charAcceptanceResult)
            {
                case CharAcceptanceResult.Continue:
                    return true;

                case CharAcceptanceResult.Stop:
                    throw new NotImplementedException(); // error in your logic.

                case CharAcceptanceResult.Fail:
                    return false;

                default:
                    throw new NotImplementedException(); // how can be?
            }
        }

        protected virtual bool ProcessEnd()
        {
            // idle, no problem for most token extractors.
            //
            // but of course will fail for unclosed strings, SQL identifiers [my_column_name , etc.
            // in such a case, throw a todo exception here.

            return true;
        }

        protected abstract bool AcceptsPreviousCharImpl(char previousChar);

        protected abstract CharAcceptanceResult AcceptCharImpl(char c, int localIndex);

        protected virtual bool IsProducer =>
            true; // most token extractors produce something; however, comment extractors do not.
    }
}
