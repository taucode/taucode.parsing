using System;
using TauCode.Utils.CommandLine.Parsing;

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
            throw new ParsingException("Cannot call 'Act' for end node.");
        }

        public override void AddLink(INode node)
        {
            throw new ParsingException("Cannot add link to end node.");
        }

        public override void AddLinkByName(string nodeName)
        {
            throw new ParsingException("Cannot add link to end node.");
        }

        public override Func<IToken, IResultAccumulator, bool> AdditionalChecker
        {
            get => null;
            set => throw new ParsingException("Cannot set 'AdditionalChecker' for end node.");
        }

        #endregion
    }
}
