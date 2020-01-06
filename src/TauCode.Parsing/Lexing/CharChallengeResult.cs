namespace TauCode.Parsing.Lexing
{
    public enum CharChallengeResult
    {
        /// <summary>
        /// Character was accepted, and token extraction process can continue.
        /// </summary>
        Continue = 1,

        /// <summary>
        /// Character is not accepted and terminates current token extraction process.
        /// </summary>
        Finish,

        /// <summary>
        /// Character cannot be part of token, but other token extractor might try to extract the whole
        /// token from initial position.
        /// </summary>
        GiveUp,

        /// <summary>
        /// Character cannot be part of token and should produce lexer error. Example: newline in string constant.
        /// </summary>
        Error, // todo: get rid of this. throw an exception within the token extractor.
    }
}
