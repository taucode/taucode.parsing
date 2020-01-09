using System;

namespace TauCode.Parsing.Lexing
{
    public struct TokenExtractionResult
    {
        public TokenExtractionResult(IToken token, int positionShift, int lineShift, int? currentColumn)
        {
            if (token == null)
            {
                var argsAreValid =
                    positionShift == 0 &&
                    lineShift == 0 &&
                    currentColumn == null;

                if (!argsAreValid)
                {
                    throw new ArgumentException("Inconsistent arguments."); // todo: arg name?
                }
            }
            else
            {
                var argsAreValid =
                    positionShift > 0 &&
                    lineShift >= 0 &&
                    currentColumn.HasValue &&
                    currentColumn.Value >= 0;

                if (!argsAreValid)
                {
                    throw new ArgumentException("Inconsistent arguments."); // todo: arg name?
                }
            }


            this.Token = token;
            this.PositionShift = positionShift;
            this.LineShift = lineShift;
            this.CurrentColumn = currentColumn;
        }

        public IToken Token { get; }
        public int PositionShift { get; }
        public int LineShift { get; }
        public int? CurrentColumn { get; }
    }
}
