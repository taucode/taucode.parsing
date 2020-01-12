using System;

namespace TauCode.Parsing.TextProcessing
{
    public struct TextProcessingResult
    {
        public static TextProcessingResult Failure { get; } = new TextProcessingResult(0, 0, null, null);

        public TextProcessingResult(int indexShift, int lineShift, int? currentColumn, IPayload payload)
        {
            if (indexShift < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(indexShift));
            }

            if (lineShift < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(lineShift));
            }

            if (currentColumn.HasValue && currentColumn.Value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(currentColumn));
            }

            if (indexShift == 0)
            {
                var validArgs =
                    lineShift == 0 &&
                    !currentColumn.HasValue &&
                    payload == null;

                if (!validArgs)
                {
                    throw new ArgumentException("Inconsistent arguments.", nameof(lineShift));
                }
            }
            else
            {
                if (!currentColumn.HasValue || payload == null)
                {
                    throw new ArgumentException("Inconsistent arguments.", nameof(lineShift));
                }
            }

            this.IndexShift = indexShift;
            this.LineShift = lineShift;
            this.CurrentColumn = currentColumn;
            this.Payload = payload;
        }

        public int IndexShift { get; }
        public int LineShift { get; }
        public int? CurrentColumn { get; }
        public IPayload Payload { get; }
    }
}
