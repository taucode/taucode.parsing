using System;

namespace TauCode.Parsing.Lexer2
{
    public class AideSyntaxTokenExtractor : TokenExtractorBase
    {
        private static readonly char[] FirstChars = { '\\' };

        public AideSyntaxTokenExtractor()
            : base(FirstChars)
        {
        }

        protected override void Reset()
        {
            // todo: idle for now...
        }

        protected override IToken ProduceResult()
        {
            throw new NotImplementedException();
        }

        protected override TestCharResult TestCurrentChar()
        {
            var localPos = this.GetLocalPosition();
            var c = this.GetCurrentChar();

            if (localPos == 0)
            {
                return this.ContinueIf(c == FirstChars[0]); // redundant, but let it be...
            }
            else if (localPos == 1)
            {
                throw new NotImplementedException();
            }

            throw new NotImplementedException();
        }
    }
}
