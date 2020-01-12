using System;
using TauCode.Parsing.TextClasses;
using TauCode.Parsing.TextDecorations;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Lexing.StandardExtractors
{
    public class WordExtractor : TokenExtractorBase<TextToken>
    {
        public override TextToken ProduceToken(string text, int absoluteIndex, Position position, int consumedLength)
        {
            var str = this.Context.Text.Substring(absoluteIndex, consumedLength);

            return new TextToken(
                WordTextClass.Instance,
                NoneTextDecoration.Instance,
                str,
                position,
                consumedLength);
        }

        protected override void OnBeforeProcess()
        {
            // todo: temporary check that IsProcessing == FALSE, everywhere
            if (this.IsProcessing)
            {
                throw new NotImplementedException();
            }

            // todo: temporary check that LocalPosition == 1, everywhere
            if (this.Context.GetLocalIndex() != 1)
            {
                throw new NotImplementedException();
            }

            // idle
        }

        protected override bool AcceptsPreviousTokenImpl(IToken previousToken)
        {
            return
                previousToken is PunctuationToken; // todo make it tunable (use list of acceptable token types in ctor).
        }

        protected override CharAcceptanceResult AcceptCharImpl(char c, int localIndex)
        {
            if (localIndex == 0)
            {
                return this.ContinueOrFail(c == '_' || LexingHelper.IsLatinLetter(c));
            }

            if (
                LexingHelper.IsInlineWhiteSpaceOrCaretControl(c) ||
                LexingHelper.IsStandardPunctuationChar(c))
            {
                return CharAcceptanceResult.Stop;
            }

            if (c == '_' || LexingHelper.IsLatinLetter(c) || LexingHelper.IsDigit(c))
            {
                return CharAcceptanceResult.Continue;
            }

            // I don't want this char inside my word.
            return CharAcceptanceResult.Fail;
        }
    }
}
