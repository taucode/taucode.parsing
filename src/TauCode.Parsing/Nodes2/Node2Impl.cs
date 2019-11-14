using System;
using System.Collections.Generic;

namespace TauCode.Parsing.Nodes2
{
    public abstract class Node2Impl : INode2
    {
        #region Fields

        

        #endregion

        #region Constructor

        protected Node2Impl(INodeFamily family, string name)
        {
            // todo: check args

            this.Family = family ?? throw new ArgumentNullException(nameof(family));
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        #endregion

        #region Polymorph

        protected abstract InquireResult InquireImpl(IToken token);

        protected abstract void ActImpl(IToken token, IResultAccumulator resultAccumulator);

        #endregion

        #region INode2 Members

        public INodeFamily Family { get; }

        public string Name { get; }

        public InquireResult Inquire(IToken token)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            return this.InquireImpl(token);
        }

        public void Act(IToken token, IResultAccumulator resultAccumulator)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (resultAccumulator == null)
            {
                throw new ArgumentNullException(nameof(resultAccumulator));
            }

            this.ActImpl(token, resultAccumulator);
        }

        public virtual void AddLink(INode2 node)
        {
            throw new NotImplementedException();
        }

        public virtual void AddLinkByName(string nodeName)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<INode2> Links => throw new NotImplementedException();

        #endregion
    }
}
