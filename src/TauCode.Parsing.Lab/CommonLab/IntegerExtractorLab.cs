using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Extensions;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Lab.CommonLab
{
    public class IntegerExtractorLab : GammaTokenExtractorBase<IntegerToken>
    {
        private readonly List<Type> _acceptablePreviousTokenTypes;

        public IntegerExtractorLab(IList<Type> acceptablePreviousTokenTypes)
        {
            _acceptablePreviousTokenTypes = acceptablePreviousTokenTypes.ToList();
        }

        public override IntegerToken ProduceToken(string text, int absoluteIndex, int consumedLength, Position position)
        {
            var signShift = 0;
            if (text[absoluteIndex] == '+')
            {
                signShift = 1;
            }

            var intSubstring = text.Substring(absoluteIndex + signShift, consumedLength - signShift);

            if (int.TryParse(intSubstring, out var dummy))
            {
                return new IntegerToken(intSubstring, position, consumedLength);
            }

            return null;
        }

        protected override bool AcceptsPreviousTokenImpl(IToken previousToken)
        {
            return _acceptablePreviousTokenTypes.Contains(previousToken.GetType());
        }

        //protected override bool AcceptsPreviousCharImpl(char previousChar)
        //{
        //    //var accepts =
        //    //    LexingHelper.IsInlineWhiteSpaceOrCaretControl(previousChar) ||
        //    //    previousChar == '"' ||
        //    //    LexingHelper.IsStandardPunctuationChar()

        //    throw new NotImplementedException();
        //}

        protected override CharAcceptanceResult AcceptCharImpl(char c, int localIndex)
        {
            if (localIndex == 0)
            {
                return this.ContinueOrFail(LexingHelper.IsIntegerFirstChar(c)); // todo: use it everywhere
            }

            if (LexingHelper.IsDigit(c))
            {
                return CharAcceptanceResult.Continue;
            }

            if (c == '.' || c == '_')
            {
                // period ('.') is rarely a thing that you can delimit an integer within any grammar.
                // underscore ('_') is a punctuation mark, but nowadays it is usually a part of identifiers.

                return CharAcceptanceResult.Fail;
            }

            // other punctuation marks like Comma, (, ), others might delimit though...
            if (char.IsPunctuation(c))
            {
                var gotOnlySign = this.GotOnlySign();
                if (gotOnlySign)
                {
                    return CharAcceptanceResult.Fail;
                }

                return CharAcceptanceResult.Stop;
            }

            if (LexingHelper.IsInlineWhiteSpaceOrCaretControl(c))
            {
                return CharAcceptanceResult.Stop;
            }

            // other chars like letters and stuff => not allowed.
            return CharAcceptanceResult.Fail;
        }

        private bool GotOnlySign()
        {
            var localIndex = this.Context.GetLocalIndex();
            if (localIndex == 0)
            {
                throw new NotImplementedException(); // not applicable. invalid call.
            }

            if (this.Context.GetLocalIndex() > 0)
            {
                var firstChar = this.Context.GetLocalChar(0);
                return firstChar.IsIn('+', '-');
            }

            return false;
        }
    }
}
