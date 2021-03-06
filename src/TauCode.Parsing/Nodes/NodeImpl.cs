﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace TauCode.Parsing.Nodes
{
    public abstract class NodeImpl : INode
    {
        #region Fields

        private readonly HashSet<INode> _establishedLinks;
        private readonly HashSet<string> _claimedLinkNames;
        private readonly IDictionary<string, string> _properties;

        #endregion

        #region Constructor

        protected NodeImpl(INodeFamily family, string name)
        {
            this.Family = family;
            this.Name = name;

            family?.RegisterNode(this);

            _establishedLinks = new HashSet<INode>();
            _claimedLinkNames = new HashSet<string>();
            _properties = new Dictionary<string, string>();
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

        protected abstract bool AcceptsTokenImpl(IToken token, IResultAccumulator resultAccumulator);

        protected abstract void ActImpl(IToken token, IResultAccumulator resultAccumulator);

        #endregion

        #region Public

        public virtual Func<IToken, IResultAccumulator, bool> AdditionalChecker { get; set; }

        #endregion

        #region INode Members

        public INodeFamily Family { get; }

        public string Name { get; }

        public bool AcceptsToken(IToken token, IResultAccumulator resultAccumulator)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (resultAccumulator == null)
            {
                throw new ArgumentNullException(nameof(resultAccumulator));
            }

            var basicResult = this.AcceptsTokenImpl(token, resultAccumulator);
            if (!basicResult)
            {
                return false;
            }

            var additionalCheck = this.AdditionalChecker?.Invoke(token, resultAccumulator) ?? true;
            return additionalCheck;
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
                throw new InvalidOperationException("This node is already linked to.");
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

        public IDictionary<string, string> Properties => _properties;

        #endregion
    }
}
