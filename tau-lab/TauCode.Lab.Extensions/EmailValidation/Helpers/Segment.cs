namespace TauCode.Lab.Extensions.EmailValidation.Helpers
{
    internal readonly struct Segment
    {
        public Segment(SegmentType type, byte start, byte length)
        {
            this.Type = type;
            this.Start = start;
            this.Length = length;
        }

        public SegmentType Type { get; }
        public byte Start { get; }
        public byte Length { get; }
    }
}
