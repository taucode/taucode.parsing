using System;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Lab.CommonLab
{
    public class IntegerExtractorLab : GammaTokenExtractorBase<IntegerToken>
    {
        public override IntegerToken ProduceToken(string text, int absoluteIndex, int consumedLength, Position position)
        {
            throw new NotImplementedException();
        }

        protected override bool AcceptsPreviousCharImpl(char previousChar)
        {
            //var accepts =
            //    LexingHelper.IsInlineWhiteSpaceOrCaretControl(previousChar) ||
            //    previousChar == '"' ||
            //    LexingHelper.IsStandardPunctuationChar()

            throw new NotImplementedException();
        }

        protected override CharAcceptanceResult AcceptCharImpl(char c, int localIndex)
        {
            throw new NotImplementedException();
        }
    }
}
