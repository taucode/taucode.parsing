using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Lexer2
{
    public class CommentExtractor : TokenExtractorBase
    {
        private static readonly char[] AllowedFirstChars = { '/' };
        //private bool _lastCharWasAsterisk;

        public CommentExtractor()
            : base(LexerHelper.StandardSpaceChars, AllowedFirstChars)
        {
        }

        protected override void Reset()
        {
            //_lastCharWasAsterisk = false;
        }

        protected override IToken ProduceResult()
        {
            return NullToken.Instance;
        }

        protected override TestCharResult TestCurrentChar()
        {
            var localPos = this.GetLocalPosition();
            var c = this.GetCurrentChar();

            if (localPos == 0)
            {
                return this.ContinueIf(c == '/');
            }
            else if (localPos == 1)
            {
                return this.ContinueIf(c == '*');
            }
            else if (localPos == 2)
            {
                return this.ContinueIf(c != '/');
            }

            if (c == '/' && this.GetPreviousChar() == '*')
            {
                this.Advance(); // todo: is this good? altering state instead of just 'testing curr char'...
                return TestCharResult.End;
            }

            return TestCharResult.Continue;

            //if (c == '*')
            //{
            //    _lastCharWasAsterisk = true;
            //    return TestCharResult.Continue;
            //}
            //else
            //{
            //    if (c == '/' && _lastCharWasAsterisk)
            //    {
            //        this.Advance(); // todo: is this good? altering state instead of just 'testing curr char'...
            //        return TestCharResult.End;
            //    }

            //    _lastCharWasAsterisk = false;
            //    return TestCharResult.Continue;
            //}
        }
    }
}
