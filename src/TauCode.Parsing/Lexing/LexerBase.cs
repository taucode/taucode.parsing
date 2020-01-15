using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.TextProcessing;

namespace TauCode.Parsing.Lexing
{
    public abstract class LexerBase : ILexer
    {
        private List<ITokenProducer> _producers;

        protected abstract ITokenProducer[] CreateProducers();
        protected List<ITokenProducer> Producers => _producers ?? (_producers = this.CreateProducers().ToList());

        public IList<IToken> Lexize(string input)
        {
            var context = new TextProcessingContext(input);
            var tokens = new List<IToken>();

            foreach (var producer in this.Producers)
            {
                producer.Context = context;
            }

            while (!context.IsEnd())
            {
                var indexBeforeProducing = context.GetIndex();

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

                if (context.GetIndex() == indexBeforeProducing)
                {
                    var position = context.GetCurrentPosition();
                    var c = context.GetCurrentChar();
                    throw new LexingException($"Unexpected char: '{c}'.", position);
                }
            }

            return tokens;
        }
    }
}
