namespace TauCode.Parsing.Lexer2
{
    public class AideSymbolExtractor : TokenExtractorBase
    {
        private static readonly char[] FirstChars = { '\\' };
        private static char[] GetSymbolChars()
        {
            return new[]
            {
                '~',
                '?',
                '!',
                '@',
                '#',
                '$',
                '%',
                '^',
                '&',
                '|',
                '*',
                '/',
                '+',
                '-',
                '(',
                ')',
                '[',
                ']',
                '{',
                '}',
                '.',
                ',',
                ':',
                ';',
                '\\',
            };
        }

        public AideSymbolExtractor(char[] spaceChars)
            : base(spaceChars, FirstChars)
        {
        }

        protected override void Reset()
        {
            throw new System.NotImplementedException();
        }

        protected override IToken ProduceResult()
        {
            throw new System.NotImplementedException();
        }

        protected override TestCharResult TestCurrentChar()
        {
            throw new System.NotImplementedException();
        }
    }
}
