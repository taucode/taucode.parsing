using System;

namespace TauCode.Parsing.Nodes
{
    public class EndNode : NodeImpl
    {
        #region Static

        public static EndNode Instance = new EndNode();

        #endregion

        #region Constructor

        private EndNode()
            : base(null, "end")
        {
        }

        #endregion

        #region Overridden

        protected override InquireResult InquireImpl(IToken token, IResultAccumulator resultAccumulator) => InquireResult.End;

        protected override void ActImpl(IToken token, IResultAccumulator resultAccumulator)
        {
            throw new NotImplementedException(); // error
        }

        public override void AddLink(INode node)
        {
            throw new NotImplementedException();
        }

        public override void AddLinkByName(string nodeName)
        {
            throw new NotImplementedException();
        }

        public override Func<IToken, IResultAccumulator, bool> AdditionalChecker
        {
            get => null;
            set => throw new NotImplementedException();
        }

        #endregion
    }
}
