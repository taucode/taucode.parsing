using TauCode.Parsing.TextClasses;
using TauCode.Parsing.TextDecorations;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Lexing.StandardProducers
{
    /// <summary>
    /// Produces a char sequence without spaces/line breaks.
    /// Might be used as a fallback producer when no other producers have accepted current input, in order to throw a lexing exception.
    /// </summary>
    public class CharSequenceProducer : ITokenProducer
    {
        public LexingContext Context { get; set; }

        public IToken Produce()
        {
            var context = this.Context;
            var text = context.Text;
            var c = text[context.Index];
            if (LexingHelper.IsInlineWhiteSpaceOrCaretControl(c))
            {
                return null;
            }

            var length = text.Length;
            var initialIndex = context.Index;
            var index = initialIndex + 1;

            while (true)
            {
                if (index == length)
                {
                    break;
                }

                c = text[index];

                if (LexingHelper.IsInlineWhiteSpaceOrCaretControl(c))
                {
                    break;
                }

                index++;
            }

            var delta = index - initialIndex;
            var str = text.Substring(initialIndex, delta);

            var position = new Position(context.Line, context.Column);
            context.Advance(delta, 0, context.Column + delta);

            var token = new TextToken(
                CharSequenceTextClass.Instance,
                NoneTextDecoration.Instance,
                str,
                position,
                delta);

            this.OnTokenProduced(token);

            return token;
        }

        protected virtual void OnTokenProduced(TextToken token)
        {
            // idle
        }
    }
}
