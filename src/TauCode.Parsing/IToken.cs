namespace TauCode.Parsing
{
    public interface IToken
    {
        /// <summary>
        /// Does the token has payload. E.g. comment tokens don't have payload.
        /// </summary>
        bool HasPayload { get; }

        /// <summary>
        /// Position within the original text.
        /// </summary>
        Position Position { get; }

        /// <summary>
        /// Length of original text consumed by the given token.
        /// </summary>
        int ConsumedLength { get; }
    }
}
