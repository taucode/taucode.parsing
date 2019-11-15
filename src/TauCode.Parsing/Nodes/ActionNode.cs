using System;

namespace TauCode.Parsing.Nodes
{
    public abstract class ActionNode : Node2Impl
    {
        protected ActionNode(
            INodeFamily family,
            string name,
            Action<IToken, IResultAccumulator> action)
            : base(family, name)
        {
            this.Action = action; // can be null
        }

        public Action<IToken, IResultAccumulator> Action { get; set; }

        protected override void ActImpl(IToken token, IResultAccumulator resultAccumulator)
        {
            if (this.Action == null)
            {
                throw new NotImplementedException(); // should not be called since action is null
            }

            this.Action(token, resultAccumulator);
            resultAccumulator.Modify();
        }
    }
}
