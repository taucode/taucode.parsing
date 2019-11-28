using System;

namespace TauCode.Parsing.Nodes
{
    public class ExactSpecialStringNode : ActionNode
    {
        public ExactSpecialStringNode(
            INodeFamily family,
            string name,
            Action<IToken, IResultAccumulator> action,
            string @class,
            string value)
            : base(family, name, action)
        {
            this.Class = @class ?? throw new ArgumentNullException(nameof(@class));
            this.Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string Class { get; }

        public string Value { get; }

        protected override InquireResult InquireImpl(IToken token, IResultAccumulator resultAccumulator)
        {
            throw new NotImplementedException();
        }
    }
}
