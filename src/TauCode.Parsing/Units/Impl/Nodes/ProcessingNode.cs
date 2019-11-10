using System;
using System.Collections.Generic;

namespace TauCode.Parsing.Units.Impl.Nodes
{
    public abstract class ProcessingNode : Node
    {
        private ProcessingNode(Action<IToken, IContext> processor)
            : base(null)
        {
            this.Processor = processor ?? throw new ArgumentNullException(nameof(processor));
        }

        protected ProcessingNode(Action<IToken, IContext> processor, string name)
            :this(processor)
        {
            this.Name = name;
        }

        protected Action<IToken, IContext> Processor { get; }

        protected abstract bool IsAcceptableToken(IToken token);

        protected override IReadOnlyList<IUnit> ProcessImpl(ITokenStream stream, IContext context)
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
