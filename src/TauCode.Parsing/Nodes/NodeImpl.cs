using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Exceptions;
using TauCode.Utils.Extensions;

namespace TauCode.Parsing.Nodes
{
    public abstract class NodeImpl : INode
    {
        #region Fields

        private readonly HashSet<INode> _establishedLinks;
        private readonly HashSet<string> _claimedLinkNames;
        private Func<IToken, IResultAccumulator, bool> _additionalChecker;

        #endregion

        #region Constructor

        protected NodeImpl(INodeFamily family, string name)
        {
            if (family != null)
            {
                if (!(family is NodeFamily))
                {
                    throw new ArgumentException(
                        $"'{nameof(family)}' must be of type '{typeof(NodeFamily).FullName}'.",
                        nameof(family));
                }
            }

            var familyImpl = (NodeFamily)family;

            this.Family = family;
            this.Name = name;

            familyImpl?.RegisterNode(this);

            _establishedLinks = new HashSet<INode>();
            _claimedLinkNames = new HashSet<string>();
        }

        #endregion

        #region Private

        private void ResolvePendingLinks()
        {
            foreach (var linkAddress in _claimedLinkNames)
            {
                var node = this.Family.GetNode(linkAddress);
                this.EstablishLink(node);
            }

            _claimedLinkNames.Clear();
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

        #region INode Members

        public INodeFamily Family { get; }

        public string Name { get; }

        public InquireResult Inquire(IToken token, IResultAccumulator resultAccumulator)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (resultAccumulator == null)
            {
                throw new ArgumentNullException(nameof(resultAccumulator));
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

        public virtual void EstablishLink(INode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            if (_establishedLinks.Contains(node))
            {
                throw new ParsingException("This node is already linked to.");
            }

            _establishedLinks.Add(node);
        }

        public virtual void ClaimLink(string nodeName)
        {
            if (nodeName == null)
            {
                throw new ArgumentNullException(nameof(nodeName));
            }

            if (_establishedLinks.Select(x => x.Name).Contains(nodeName))
            {
                throw new ArgumentException($"Node is already linked to a node with name '{nodeName}'.");
            }

            if (_claimedLinkNames.Contains(nodeName))
            {
                throw new ArgumentException($"Node is already linked to a node with name '{nodeName}'.");
            }

            _claimedLinkNames.Add(nodeName);
        }

        public virtual IReadOnlyCollection<INode> EstablishedLinks => _establishedLinks;

        public IReadOnlyCollection<string> ClaimedLinkNames => _claimedLinkNames;

        public IReadOnlyCollection<INode> ResolveLinks()
        {
            if (_claimedLinkNames.Count > 0)
            {
                this.ResolvePendingLinks();
            }

            return _establishedLinks;
        }

        #endregion
    }
}
