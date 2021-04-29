using TauCode.Parsing.Lexing;

namespace TauCode.Parsing.TinyLisp.Producers
{
    public class TinyLispCommentProducer : ITokenProducer
    {
        public LexingContext Context { get; set; }

        public IToken Produce()
        {
            var context = this.Context;
            var text = context.Text;
            var length = context.Length;

            var c = text[context.Index];

            if (c == ';')
            {
                var initialIndex = context.Index;
                var index = initialIndex + 1; // skip ';'
                var column = context.Column + 1; // skip ';'

                while (true)
                {
                    if (index == length)
                    {
                        context.Advance(index - initialIndex, 0, column);
                        return null;
                    }

                    c = text[index];
                    if (LexingHelper.IsCaretControl(c))
                    {
                        context.Advance(index - initialIndex, 0, column);
                        return null;
                    }

                    index++;
                    column++;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
