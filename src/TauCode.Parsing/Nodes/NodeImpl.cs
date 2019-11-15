using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Utils.Extensions;

namespace TauCode.Parsing.Nodes
{
    public abstract class NodeImpl : INode
    {
        #region Fields

        private readonly HashSet<INode> _links;
        private readonly HashSet<string> _linkAddresses;
        private Func<IToken, IResultAccumulator, bool> _additionalChecker;

        #endregion

        #region Constructor

        protected NodeImpl(INodeFamily family, string name)
        {
            // todo: check args
            var familyImpl = (NodeFamily)family; // todo check

            this.Family = family;
            this.Name = name ?? throw new ArgumentNullException(nameof(name));

            familyImpl?.RegisterNode(this);

            _links = new HashSet<INode>();
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

            _linkAddresses.Clear();
        }

        #endregion
        
        #region Polymorph

        protected abstract InquireResult InquireImpl(IToken token, IResultAccumulator resultAccumulator);

        protected abstract void ActImpl(IToken token, IResultAccumulator resultAccumulator);

        #endregion

        #region Public

        public virtual Func<IToken, IResultAccumulator, bool> AdditionalChecker
        {
            get => _additionalChecker;
            set => _additionalChecker = value;
        }

        #endregion

        #region INode2 Members

        public INodeFamily Family { get; }

        public string Name { get; }

        public InquireResult Inquire(IToken token, IResultAccumulator resultAccumulator)
        {
            // todo checks
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            var basicInquireResult = this.InquireImpl(token, resultAccumulator);

            if (basicInquireResult.IsIn(InquireResult.Reject, InquireResult.End))
            {
                return basicInquireResult;
            }

            var additionalCheck = this.AdditionalChecker?.Invoke(token, resultAccumulator) ?? true;
            return additionalCheck ? basicInquireResult : InquireResult.Reject;
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

        public virtual void AddLink(INode node)
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

        public virtual IReadOnlyCollection<INode> Links
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
