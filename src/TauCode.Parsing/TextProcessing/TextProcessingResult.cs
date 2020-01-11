using System;

namespace TauCode.Parsing.TextProcessing
{
    public struct TextProcessingResult
    {
        public static TextProcessingResult Fail { get; } =
            new TextProcessingResult(TextProcessingSummary.Fail, 0, 0, null);

        public TextProcessingResult(TextProcessingSummary summary, int indexShift, int lineShift, int? currentColumn)
        {
            if (summary == TextProcessingSummary.Skip || summary == TextProcessingSummary.CanProduce)
            {
                var argsAreValid =
                    indexShift > 0 &&
                    currentColumn.HasValue;

                if (!argsAreValid)
                {
                    throw new NotImplementedException(); // todo
                }
            }
            else if (summary == TextProcessingSummary.Fail)
            {
                var argsAreValid =
                    indexShift == 0 &&
                    lineShift == 0 &&
                    !currentColumn.HasValue;

                if (!argsAreValid)
                {
                    throw new NotImplementedException(); // todo
                }
            }
            else
            {
                throw new NotImplementedException(); // a kto zhe togda?! :)
            }

            this.Summary = summary;
            this.IndexShift = indexShift;
            this.LineShift = lineShift;
            this.CurrentColumn = currentColumn;
        }

        public TextProcessingSummary Summary { get; }
        public int IndexShift { get; set; }
        public int LineShift { get; set; }
        public int? CurrentColumn { get; set; }

        public int GetCurrentColumn() =>
            this.CurrentColumn ??
            throw new NotImplementedException(); // bad operation; something wrong with your logic.
    }
}
