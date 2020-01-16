using System;

namespace TauCode.Parsing.Nodes
{
    // todo clean
    public class EndNode : NodeImpl
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

        protected override /*InquireResult*/ bool InquireImpl(IToken token, IResultAccumulator resultAccumulator) => 
            //InquireResult.End;
            throw new NotImplementedException(); // todo should not be called

        protected override void ActImpl(IToken token, IResultAccumulator resultAccumulator)
        {
            throw new InvalidOperationException("Cannot call 'Act' for end node.");
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
            set => throw new InvalidOperationException("Cannot set 'AdditionalChecker' for end node.");
        }

        #endregion
    }
}
