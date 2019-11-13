using System;
using System.Collections.Generic;

namespace TauCode.Parsing.Units.Impl.Nodes
{
    public abstract class ProcessingNode : Node
    {
        protected ProcessingNode(Action<IToken, IContext> processor, string name)
            : base(name)
        {
            this.Processor = processor ?? throw new ArgumentNullException(nameof(processor));
        }

        protected Action<IToken, IContext> Processor { get; }

        protected abstract bool IsAcceptableToken(IToken token);

        protected override IReadOnlyCollection<IUnit> ProcessImpl(ITokenStream stream, IContext context)
        {
            var token = stream.GetCurrentToken();
            if (this.IsAcceptableToken(token))
            {
                this.Processor(token, context);
                stream.AdvanceStreamPosition();
                return this.Links;
            }

            return null;
        }
    }
}
