using System;

namespace TauCode.Parsing.Tests.Tokens
{
    public class WordToken : IToken
    {
        public WordToken(string word)
        {
            this.Word = word ?? throw new ArgumentNullException(nameof(word));
        }

        public string Word { get; }
    }
}
