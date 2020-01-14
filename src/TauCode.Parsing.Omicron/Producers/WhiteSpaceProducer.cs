using TauCode.Parsing.Lexing;
using TauCode.Parsing.TextProcessing;

namespace TauCode.Parsing.Omicron.Producers
{
    public class WhiteSpaceProducer : IOmicronTokenProducer
    {
        public TextProcessingContext Context { get; set; }

        public IToken Produce()
        {
            var initialIndex = this.Context.GetIndex();
            var text = this.Context.Text;
            var length = text.Length;
            var currentIndex = initialIndex;

            var lineShift = 0;
            var column = this.Context.Column;

            while (true)
            {
                if (currentIndex == length)
                {
                    this.Context.Advance(currentIndex - initialIndex, lineShift, column);
                    return null;
                }

                var c = text[currentIndex];
                switch (c)
                {
                    case '\t':
                    case ' ':
                        currentIndex++;
                        column++;
                        break;

                    case LexingHelper.CR:
                        currentIndex++;
                        lineShift++;
                        column = 0;

                        if (currentIndex < length)
                        {
                            var nextChar = text[currentIndex];
                            if (nextChar == LexingHelper.LF)
                            {
                                currentIndex++;
                            }
                        }

                        break;

                    case LexingHelper.LF:
                        currentIndex++;
                        lineShift++;
                        column = 0;
                        break;

                    default:
                        if (currentIndex > initialIndex)
                        {
                            this.Context.Advance(currentIndex - initialIndex, lineShift, column);
                        }

                        return null;
                }
            }
        }
    }
}
