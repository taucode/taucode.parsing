using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Exceptions;

namespace TauCode.Parsing.Lexing
{
    public abstract class LexerBase : ILexer
    {
        private List<ITokenProducer> _producers;

        protected abstract ITokenProducer[] CreateProducers();
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
                    var version = context.Version;
                    var token = producer.Produce();
                    if (token != null)
                    {
                        tokens.Add(token);
                        break;
                    }

                    if (context.Version > version)
                    {
                        break;
                    }
                }

                if (context.Index == indexBeforeProducing)
                {
                    var position = new Position(context.Line, context.Column);
                    var c = input[context.Index];
                    throw new LexingException($"Unexpected char: '{c}'.", position);
                }
            }

            return tokens;
        }
    }
}
