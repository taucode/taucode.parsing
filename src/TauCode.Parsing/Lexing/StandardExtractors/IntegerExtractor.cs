using System;
using TauCode.Extensions;
using TauCode.Parsing.TextProcessing;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Lexing.StandardExtractors
{
    public class IntegerExtractor : TokenExtractorBase<IntegerToken>
    {
        private char? _sign;

        public IntegerExtractor(params Type[] acceptablePreviousTokenTypes) // todo: why params?
            : base(acceptablePreviousTokenTypes)
        {

        }

        protected override void OnBeforeProcess()
        {
            var possibleSign = this.Context.GetCharAtOffset(0);
            if (possibleSign.IsIn('+', '-'))
            {
                _sign = possibleSign;
            }
            else
            {
                _sign = null;
            }
        }

        protected override CharAcceptanceResult AcceptCharImpl(char c, int localIndex)
        {
            if (localIndex == 0)
            {
                return this.ContinueOrFail(LexingHelper.IsIntegerFirstChar(c));
            }

            if (LexingHelper.IsDigit(c))
            {
                return CharAcceptanceResult.Continue;
            }

            if (c == '.' || c == '_')
            {
                // period ('.') is rarely a thing that you can use to delimit an integer within any grammar.
                // underscore ('_') is a punctuation mark, but nowadays it is usually a part of identifiers or other words.
                return CharAcceptanceResult.Fail;
            }

            // other punctuation marks like Comma, (, ), others might delimit though...
            if (char.IsPunctuation(c))
            {
                if (_sign.HasValue)
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

        protected override IntegerToken DeliverToken(string text, int absoluteIndex, Position position, int consumedLength)
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
    }
}
