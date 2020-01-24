using System;
using TauCode.Parsing.Nodes;

namespace TauCode.Parsing.Exceptions
{
    public class FallbackNodeAcceptedTokenException : ParsingExceptionBase
    {
        public FallbackNodeAcceptedTokenException(
            FallbackNode fallbackNode,
            IToken token,
            IResultAccumulator resultAccumulator)
            : base("Fallback node accepted token.")
        {
            this.FallbackNode = fallbackNode ?? throw new ArgumentNullException(nameof(fallbackNode));
            this.Token = token ?? throw new ArgumentNullException(nameof(token));
            this.ResultAccumulator = resultAccumulator ?? throw new ArgumentNullException(nameof(resultAccumulator));
        }

        public FallbackNode FallbackNode { get; }

        public IToken Token { get; }

        public IResultAccumulator ResultAccumulator { get; }
    }
}
