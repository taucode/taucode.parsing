using System;

namespace TauCode.Parsing
{
    public static class ParsingHelper
    {
        public static bool IsEndOfStream(this ITokenStream tokenStream)
        {
            if (tokenStream == null)
            {
                throw new ArgumentNullException(nameof(tokenStream));
            }

            return tokenStream.Position == tokenStream.Tokens.Count;
        }

        public static IToken GetCurrentToken(this ITokenStream tokenStream)
        {
            if (tokenStream.IsEndOfStream())
            {
                throw new NotImplementedException();
            }

            var token = tokenStream.Tokens[tokenStream.Position];
            return token;
        }

        public static void AdvanceStreamPosition(this ITokenStream tokenStream)
        {
            if (tokenStream == null)
            {
                throw new ArgumentNullException(nameof(tokenStream));
            }

            tokenStream.Position++;
        }

        public static void IdleTokenProcessor(IToken token, IParsingContext context)
        {
        }

    }
}
