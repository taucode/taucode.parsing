using System;
using System.Collections.Generic;

namespace TauCode.Parsing
{
    public class Context2 : IContext2
    {
        public Context2(ITokenStream tokenStream)
        {
            this.TokenStream = tokenStream ?? throw new ArgumentNullException(nameof(tokenStream));
        }

        public ITokenStream TokenStream { get; }

        public void SetNodes(params INode2[] nodes)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<INode2> GetNodes()
        {
            throw new NotImplementedException();
        }
    }
}
