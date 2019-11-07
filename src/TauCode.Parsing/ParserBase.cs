using System;
using System.Collections.Generic;
using TauCode.Parsing.ParsingUnits;

namespace TauCode.Parsing
{
    public abstract class ParserBase : IParser
    {
        #region Protected

        protected IParsingUnit Head { get; private set; }

        #endregion

        #region Abstract

        protected abstract IParsingUnit BuildTree();

        #endregion

        #region IParser Members

        public IParsingContext Parse(IEnumerable<IToken> tokens)
        {
            var context = new ParsingContext();
            var stream = new TokenStream(tokens);

            if (this.Head == null)
            {
                this.Head = this.BuildTree();
            }

            var current = this.Head;

            do
            {
                var result = current.Process(stream, context);
                if (result == null)
                {
                    throw new NotImplementedException();
                }
                else if (result.Count == 1)
                {
                    if (ParsingHelper.IsEndResult(result))
                    {
                        return context;
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }

                throw new NotImplementedException();

            } while (true);
        }

        #endregion
    }
}
