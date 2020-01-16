using System;

namespace TauCode.Parsing.Nodes
{
    public sealed class EndNode : NodeImpl
    {
        #region Static

        public static EndNode Instance { get; } = new EndNode();

        #endregion

        #region Constructor

        private EndNode()
            : base(null, "<End>")
        {
        }

        #endregion

        #region Overridden

        protected override bool AcceptsTokenImpl(IToken token, IResultAccumulator resultAccumulator)
        {
            throw new InvalidOperationException($"Cannot call '{nameof(AcceptsToken)}' for end node.");
        }

        protected override void ActImpl(IToken token, IResultAccumulator resultAccumulator)
        {
            throw new InvalidOperationException($"Cannot call '{nameof(Act)}' for end node.");
        }

        public override void EstablishLink(INode node)
        {
            throw new InvalidOperationException("Cannot add link to end node.");
        }

        public override void ClaimLink(string nodeName)
        {
            throw new InvalidOperationException("Cannot add link to end node.");
        }

        public override Func<IToken, IResultAccumulator, bool> AdditionalChecker
        {
            get => null;
            set => throw new InvalidOperationException($"Cannot set '{nameof(AdditionalChecker)}' for end node.");
        }

        #endregion
    }
}
