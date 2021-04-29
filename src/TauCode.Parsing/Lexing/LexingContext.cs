using System;

namespace TauCode.Parsing.Lexing
{
    public class LexingContext
    {
        public LexingContext(string text, int? length = null)
        {
            this.Text = text ?? throw new ArgumentNullException();

            if (length.HasValue)
            {
                if (length.Value < 0 || length.Value > text.Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(length));
                }
            }

            this.Length = length ?? this.Text.Length;

            this.Version = 1;
        }

        public string Text { get; }

        public int Length { get; }

        public int Version { get; private set; }

        public int Index;
        public int Line;
        public int Column;

        internal void IncreaseVersion()
        {
            this.Version++;
        }
    }
}
