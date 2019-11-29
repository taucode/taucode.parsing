using System;
using TauCode.Parsing.Exceptions;

namespace TauCode.Parsing.Nodes
{
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

        protected override InquireResult InquireImpl(IToken token, IResultAccumulator resultAccumulator) => InquireResult.End;

        protected override void ActImpl(IToken token, IResultAccumulator resultAccumulator)
        {
            throw new ParserException("Cannot call 'Act' for end node.");
        }

        public override void EstablishLink(INode node)
        {
            throw new ParserException("Cannot add link to end node.");
        }

        public override void ClaimLink(string nodeName)
        {
            throw new ParserException("Cannot add link to end node.");
        }

        public override Func<IToken, IResultAccumulator, bool> AdditionalChecker
        {
            get => null;
            set => throw new ParserException("Cannot set 'AdditionalChecker' for end node.");
        }

        #endregion
    }
}
