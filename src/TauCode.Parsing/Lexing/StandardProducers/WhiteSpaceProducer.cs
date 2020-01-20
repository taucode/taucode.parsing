namespace TauCode.Parsing.Lexing.StandardProducers
{
    public class WhiteSpaceProducer : ITokenProducer
    {
        public LexingContext Context { get; set; }

        public IToken Produce()
        {
            var initialIndex = this.Context.Index;
            var text = this.Context.Text;
            var c = text[initialIndex];
            if (!LexingHelper.IsInlineWhiteSpaceOrCaretControl(c))
            {
                return null;
            }

            var length = text.Length;
            var currentIndex = initialIndex;
            
            var initialLine = this.Context.Line;
            var initialColumn = this.Context.Column;
            var lineShift = 0;
            var column = this.Context.Column;


            while (true)
            {
                if (currentIndex == length)
                {
                    this.Context.Advance(currentIndex - initialIndex, lineShift, column);
                    return null;
                }

                c = text[currentIndex];
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
                        var delta = currentIndex - initialIndex;

                        var token = this.ProduceImpl(new Position(initialLine, initialColumn), delta);
                        if (currentIndex > initialIndex)
                        {
                            this.Context.Advance(delta, lineShift, column);
                        }

                        return token;
                }
            }
        }

        protected virtual IToken ProduceImpl(Position position, int delta)
        {
            return null;
        }
    }
}
