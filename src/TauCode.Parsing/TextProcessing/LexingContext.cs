using System;

namespace TauCode.Parsing.TextProcessing
{
    public class LexingContext
    {
        public LexingContext(string text)
        {
            this.Text = text ?? throw new ArgumentNullException();
            this.Length = this.Text.Length;

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
