namespace TauCode.Parsing.Lexing.StandardProducers
{
    public class WhiteSpaceProducer : ITokenProducer
    {
        public LexingContext Context { get; set; }

        public IToken Produce()
        {
            var context = this.Context;
            var initialIndex = context.Index;
            var text = context.Text;
            var length = context.Length;

            var c = text[initialIndex];
            if (!LexingHelper.IsInlineWhiteSpaceOrCaretControl(c))
            {
                return null;
            }


            
            var currentIndex = initialIndex;
            
            var initialLine = context.Line;
            var initialColumn = context.Column;
            var lineShift = 0;
            var column = context.Column;


            while (true)
            {
                if (currentIndex == length)
                {
                    context.Advance(currentIndex - initialIndex, lineShift, column);
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
                            context.Advance(delta, lineShift, column);
                        }

                        return token;
                }
            }
        }

        protected virtual IToken ProduceImpl(Position position, int delta)
        {
            return null; // returns null token, but Context advanced, i.e. white space was skipped.
        }
    }
}
