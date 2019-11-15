using System;
using System.Collections.Generic;
using System.Linq;

namespace TauCode.Parsing.Nodes2
{
    public abstract class Node2Impl : INode2
    {
        #region Fields

        private readonly HashSet<INode2> _links;
        private readonly HashSet<string> _linkAddresses;

        #endregion

        #region Constructor

        protected Node2Impl(INodeFamily family, string name)
        {
            // todo: check args
            var familyImpl = (NodeFamily)family; // todo check

            this.Family = family;
            this.Name = name ?? throw new ArgumentNullException(nameof(name));

            familyImpl?.RegisterNode(this);

            _links = new HashSet<INode2>();
            _linkAddresses = new HashSet<string>();
        }

        #endregion

        #region Private

        private void ResolvePendingLinks()
        {
            foreach (var linkAddress in _linkAddresses)
            {
                var node = this.Family.GetNode(linkAddress);
                this.AddLink(node);
            }
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
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            if (_links.Contains(node))
            {
                throw new NotImplementedException();
            }

            _links.Add(node);
        }

        public virtual void AddLinkByName(string nodeName)
        {
            // todo check args
            if (_links.Select(x => x.Name).Contains(nodeName))
            {
                throw new NotImplementedException();
            }

            if (_linkAddresses.Contains(nodeName))
            {
                throw new NotImplementedException();
            }

            _linkAddresses.Add(nodeName);
        }

        public IReadOnlyCollection<INode2> Links
        {
            get
            {
                if (_linkAddresses.Any()) // todo optimize
                {
                    this.ResolvePendingLinks();
                }

                return _links;
            }
        }

        #endregion
    }
}
