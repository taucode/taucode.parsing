using System;

namespace TauCode.Parsing.Aide.Tokens
{
    public class WordAideToken : AideToken
    {
        public WordAideToken(string word, string name)
            : base(name)
        {
            this.Word = word ?? throw new ArgumentNullException(nameof(word));
        }

        public string Word { get; }
    }
}
