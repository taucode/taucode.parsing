using System;

namespace TauCode.Parsing.Old.Lexing
{
    public struct OldTokenExtractionResult
    {
        public OldTokenExtractionResult(IToken token, int positionShift, int lineShift, int? currentColumn)
        {
            if (token == null)
            {
                var argsAreValid =
                    positionShift == 0 &&
                    lineShift == 0 &&
                    currentColumn == null;

                if (!argsAreValid)
                {
                    throw new ArgumentException("Inconsistent arguments.", nameof(token));
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
                    throw new ArgumentException("Inconsistent arguments.", nameof(token));
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
