using TauCode.Parsing.Lexing;
using TauCode.Parsing.TextProcessing;

namespace TauCode.Parsing.TinyLisp.Producers
{
    public class TinyLispCommentProducer : ITokenProducer
    {
        public ITextProcessingContext Context { get; set; }

        public IToken Produce()
        {
            var c = this.Context.GetCurrentChar();

            if (c == ';')
            {
                var text = this.Context.Text;
                var length = text.Length;
                var initialIndex = this.Context.GetIndex();
                var index = initialIndex + 1; // skip ';'
                var column = this.Context.Column + 1; // skip ';'

                while (true)
                {
                    if (index == length)
                    {
                        this.Context.Advance(index - initialIndex, 0, column);
                        return null;
                    }

                    c = text[index];
                    if (LexingHelper.IsCaretControl(c))
                    {
                        this.Context.Advance(index - initialIndex, 0, column);
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
