namespace TauCode.Parsing.TextProcessing
{
    // todo clean
    public interface ITextProcessingContext
    {
        string Text { get; }

        void RequestGeneration();

        void ReleaseGeneration();

        int Depth { get; }

        int Version { get; }

        int GetCurrentLine();

        int GetAbsoluteIndex();

        Position GetCurrentAbsolutePosition();

        int GetCurrentColumn();

        int GetStartIndex();

        int GetLocalIndex();

        bool IsEnd();

        void Advance(int indexShift, int lineShift, int currentColumn);

        char GetCurrentChar();

        char GetLocalChar(int localIndex);

        //void AdvanceByChar();

        char? TryGetNextLocalChar();

        char? TryGetPreviousLocalChar();
    }
}
