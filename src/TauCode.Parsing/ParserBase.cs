using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.ParsingUnits;
using TauCode.Parsing.ParsingUnits.Impl.Nodes;

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
                    if (result.Count == 2 && result.Contains(EndParsingNode.Instance))
                    {
                        result = result.Where(x => x != EndParsingNode.Instance).ToList();
                        if (result.Count != 1)
                        {
                            throw new NotImplementedException();
                        }

                        continue;
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }

                throw new NotImplementedException();

            } while (true);
        }

        #endregion
    }
}
