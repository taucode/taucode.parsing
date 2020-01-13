namespace TauCode.Parsing.TextProcessing
{
    public interface ITextProcessingContext
    {
        string Text { get; }

        void RequestGeneration();

        void ReleaseGeneration();

        int Depth { get; }

        int Version { get; }

        int StartIndex { get; }

        int IndexOffset { get; }

        int Line { get; }

        int Column { get; }

        void Advance(int indexShift, int lineShift, int column);
    }
}
