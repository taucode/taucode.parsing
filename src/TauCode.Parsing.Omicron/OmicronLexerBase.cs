using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.TextProcessing;

namespace TauCode.Parsing.Omicron
{
    public abstract class OmicronLexerBase : ILexer
    {
        private List<IOmicronTokenProducer> _producers;

        protected abstract IOmicronTokenProducer[] CreateProducers();
        protected List<IOmicronTokenProducer> Producers => _producers ?? (_producers = this.CreateProducers().ToList());

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
                    throw new NotImplementedException(); // could not lexize.
                }
            }

            return tokens;
        }
    }
}
