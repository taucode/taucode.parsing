using System;
using TauCode.Parsing.Lexing;

namespace TauCode.Parsing.Tests.Parsing
{
    public class SqlLexer : LexerBase
    {
        public SqlLexer(Func<char, bool> spacePredicate, Func<char, bool> lineBreakPredicate)
            : base(LexingHelper.IsSpace, LexingHelper.IsLineBreak)
        {
        }

        protected override void InitTokenExtractors()
        {
            throw new NotImplementedException();
        }
    }
}
