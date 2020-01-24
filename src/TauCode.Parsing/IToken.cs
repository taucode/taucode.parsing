namespace TauCode.Parsing
{
    public interface IToken
    {
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
