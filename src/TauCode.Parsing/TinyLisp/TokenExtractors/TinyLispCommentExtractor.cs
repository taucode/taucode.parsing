﻿using TauCode.Parsing.Lexer2;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.TinyLisp.TokenExtractors
{
    public class TinyLispCommentExtractor : TokenExtractorBase
    {
        public TinyLispCommentExtractor()
            : base(
                TinyLispHelper.SpaceChars,
                TinyLispHelper.LineBreakChars,
                new[] { ';' })
        {
        }

        protected override void Reset()
        {
            //
        }

        protected override IToken ProduceResult()
        {
            var str = this.ExtractResultString();
            return new CommentToken(str);
        }

        protected override TestCharResult TestCurrentChar()
        {
            var c = this.GetCurrentChar();
            var pos = this.GetLocalPosition();

            if (pos == 0)
            {
                return this.ContinueIf(c == ';');
            }

            if (this.IsLineBreakChar(c))
            {
                return TestCharResult.End;
            }

            return TestCharResult.Continue;
        }

        protected override bool TestEnd() => true;
    }
}
