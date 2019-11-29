using TauCode.Parsing.Lexer2;
using TauCode.Parsing.TinyLisp.TokenExtractors;

namespace TauCode.Parsing.TinyLisp
{
    public class TinyLispLexer : LexerBase
    {
        public TinyLispLexer()
            : base(
                TinyLispHelper.SpaceChars,
                TinyLispHelper.LineBreakChars)
        {
        }

        public bool AddCommentTokens { get; set; }

        protected override void InitTokenExtractors()
        {
            var commentExtractor = new TinyLispCommentExtractor();

            this.AddTokenExtractor(commentExtractor);
        }
    }
}
