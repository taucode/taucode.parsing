using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Exceptions;

namespace TauCode.Parsing.Lexing
{
    public abstract class LexerBase : ILexer
    {
        private List<ITokenProducer> _producers;

        protected LexerBase(bool ignoreEmptyTokens = true)
        {
            this.IgnoreEmptyTokens = ignoreEmptyTokens;
        }

        protected abstract ITokenProducer[] CreateProducers();

        protected bool IgnoreEmptyTokens { get; }

        protected List<ITokenProducer> Producers => _producers ?? (_producers = this.CreateProducers().ToList());

        public IList<IToken> Lexize(string input)
        {
            var context = new LexingContext(input);
            var tokens = new List<IToken>();

            foreach (var producer in this.Producers)
            {
                producer.Context = context;
            }

            var length = input.Length;
            while (context.Index < length)
            {
                var indexBeforeProducing = context.Index;

                foreach (var producer in this.Producers)
                {
                    var oldVersion = context.Version;
                    var oldIndex = context.Index;
                    var oldLine = context.Line;

                    var token = producer.Produce();
                    if (token != null)
                    {
                        if (context.Version <= oldVersion)
                        {
                            throw new LexingException(
                                $"Producer '{producer.GetType().FullName}' has produced a token of type '{token.GetType().FullName}' ('{token}'), but context version has not increased.",
                                new Position(context.Length, context.Column));
                        }

                        if (context.Index <= oldIndex)
                        {
                            throw new LexingException(
                                $"Producer '{producer.GetType().FullName}' has produced a token of type '{token.GetType().FullName}' ('{token}'), but context index has not increased.",
                                new Position(context.Length, context.Column));
                        }

                        if (context.Line < oldLine)
                        {
                            throw new LexingException(
                                $"Producer '{producer.GetType().FullName}' has produced a token of type '{token.GetType().FullName}' ('{token}'), but context line has decreased.",
                                new Position(context.Length, context.Column));
                        }

                        if (token is IEmptyToken && this.IgnoreEmptyTokens)
                        {
                            // do nothing
                        }
                        else
                        {
                            tokens.Add(token);
                        }

                        break;
                    }

                    if (context.Version > oldVersion)
                    {
                        break;
                    }
                }

                if (context.Index == indexBeforeProducing)
                {
                    var position = new Position(context.Line, context.Column);
                    var c = input[context.Index];
                    throw new LexingException($"Could not lexize starting from char '{c}'. See '{nameof(LexingException.Position)}' property to get more information.", position);
                }
            }

            return tokens;
        }
    }
}
