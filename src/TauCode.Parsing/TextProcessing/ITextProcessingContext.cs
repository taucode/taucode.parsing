namespace TauCode.Parsing.TextProcessing
{
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

        int GetStartingIndex();

        int GetLocalIndex();

        bool IsEnd();

        void Advance(int indexShift, int lineShift, int currentColumn);

        char GetCurrentChar();

        char GetLocalChar(int localIndex);

        void AdvanceByChar();

        // todo: rename to TryGetPreviousAbsoluteChar
        //char? GetPreviousAbsoluteChar();

        char? TryGetNextLocalChar();
        char? TryGetPreviousLocalChar();
    }
}
