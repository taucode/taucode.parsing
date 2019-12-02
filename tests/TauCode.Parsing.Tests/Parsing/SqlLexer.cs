using System;
using TauCode.Parsing.Lexizing;

namespace TauCode.Parsing.Tests.Parsing
{
    public class SqlLexer : LexerBase
    {
        public SqlLexer(Func<char, bool> spacePredicate, Func<char, bool> lineBreakPredicate)
            : base(LexizingHelper.IsSpace, LexizingHelper.IsLineBreak)
        {
        }

        protected override void InitTokenExtractors()
        {
            throw new NotImplementedException();
        }
    }
}
