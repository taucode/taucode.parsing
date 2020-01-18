using System;

namespace TauCode.Parsing.Nodes
{
    public class CustomActionNode : ActionNode
    {
        public CustomActionNode(
            Action<ActionNode, IToken, IResultAccumulator> action,
            Func<IToken, IResultAccumulator, bool> tokenAcceptPredicate,
            INodeFamily family,
            string name)
            : base(action, family, name)
        {
            this.TokenAcceptPredicate = tokenAcceptPredicate;
        }

        public Func<IToken, IResultAccumulator, bool> TokenAcceptPredicate { get; set; }

        protected override bool AcceptsTokenImpl(IToken token, IResultAccumulator resultAccumulator)
        {
            if (this.TokenAcceptPredicate == null)
            {
                throw new InvalidOperationException($"'{nameof(TokenAcceptPredicate)}' is null.");
            }

            return this.TokenAcceptPredicate(token, resultAccumulator);
        }
    }
}
