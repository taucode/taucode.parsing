using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Utils.CommandLine.Parsing;
using TauCode.Utils.Extensions;

namespace TauCode.Parsing.Nodes
{
    public abstract class NodeImpl : INode
    {
        #region Fields

        private readonly HashSet<INode> _links;
        private readonly HashSet<string> _linkedNodeNames;
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
            this.Name = name ?? throw new ArgumentNullException(nameof(name));

            familyImpl?.RegisterNode(this);

            _links = new HashSet<INode>();
            _linkedNodeNames = new HashSet<string>();
        }

        #endregion

        #region Private

        private void ResolvePendingLinks()
        {
            foreach (var linkAddress in _linkedNodeNames)
            {
                var node = this.Family.GetNode(linkAddress);
                this.AddLink(node);
            }

            _linkedNodeNames.Clear();
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

        public virtual void AddLink(INode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            if (_links.Contains(node))
            {
                throw new ParsingException("This is node is already linked to.");
            }

            _links.Add(node);
        }

        public virtual void AddLinkByName(string nodeName)
        {
            if (nodeName == null)
            {
                throw new ArgumentNullException(nameof(nodeName));
            }

            if (_links.Select(x => x.Name).Contains(nodeName))
            {
                throw new ArgumentException($"Node is already linked to a node with name '{nodeName}'.");
            }

            if (_linkedNodeNames.Contains(nodeName))
            {
                throw new ArgumentException($"Node is already linked to a node with name '{nodeName}'.");
            }

            _linkedNodeNames.Add(nodeName);
        }

        public virtual IReadOnlyCollection<INode> Links
        {
            get
            {
                if (_linkedNodeNames.Count > 0)
                {
                    this.ResolvePendingLinks();
                }

                return _links;
            }
        }

        #endregion
    }
}
