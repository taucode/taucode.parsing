using System;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.TextProcessing;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Omicron.Producers
{
    public class IntegerProducer : IOmicronTokenProducer
    {
        private readonly Func<char, bool> _acceptableTerminatorPredicate;

        public IntegerProducer(Func<char, bool> acceptableTerminatorPredicate)
        {
            _acceptableTerminatorPredicate = acceptableTerminatorPredicate;
        }

        public TextProcessingContext Context { get; set; }

        public IToken Produce()
        {
            var c = this.Context.GetCurrentChar();

            if (LexingHelper.IsIntegerFirstChar(c))
            {
                var context = this.Context;
                var text = context.Text;
                var length = text.Length;

                var initialIndex = context.GetIndex();
                var initialColumn = context.Column;

                var digitsCount = 1;
                if (c == '+' || c == '-')
                {
                    digitsCount = 0;
                }

                var gotPlusSign = c == '+';

                var index = initialIndex + 1;
                var column = this.Context.Column + 1;

                while (true)
                {
                    if (index == length)
                    {
                        break;
                    }

                    c = text[index];
                    if (LexingHelper.IsDigit(c))
                    {
                        index++;
                        column++;
                        digitsCount++;
                    }
                    else
                    {
                        if (_acceptableTerminatorPredicate(c))
                        {
                            break;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }

                if (digitsCount == 0)
                {
                    return null;
                }

                var delta = index - initialIndex;
                var defect = gotPlusSign ? 1 : 0;
                var intString = text.Substring(initialIndex + defect, delta - defect);
                if (int.TryParse(intString, out var dummy))
                {
                    this.Context.Advance(delta, 0, column);
                    return new IntegerToken(intString, new Position(this.Context.Line, initialColumn), delta);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
