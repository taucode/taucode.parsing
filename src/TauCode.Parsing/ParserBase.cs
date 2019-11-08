using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Units;
using TauCode.Parsing.Units.Impl.Nodes;

namespace TauCode.Parsing
{
    public abstract class ParserBase : IParser
    {
        #region Protected

        protected IUnit Head { get; private set; }

        #endregion

        #region Abstract

        protected abstract IUnit BuildTree();

        #endregion

        #region IParser Members

        public IContext Parse(IEnumerable<IToken> tokens)
        {
            var context = new Context();
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
                    if (result.Count == 2 && result.Contains(EndNode.Instance))
                    {
                        result = result.Where(x => x != EndNode.Instance).ToList();
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
