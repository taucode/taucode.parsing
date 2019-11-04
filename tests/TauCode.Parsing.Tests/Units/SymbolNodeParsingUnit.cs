using System;
using TauCode.Parsing.ParsingUnits;
using TauCode.Parsing.Tests.Tokens;

namespace TauCode.Parsing.Tests.Units
{
    public class SymbolNodeParsingUnit : NodeParsingUnit
    {
        public SymbolNodeParsingUnit(SymbolTokenValue value, Action<IToken, IParsingContext> processor)
            : base(processor)
        {
            this.Value = value;
        }

        public SymbolNodeParsingUnit(char c, Action<IToken, IParsingContext> processor)
            : this(Helper.SymbolTokenFromChar(c), processor)
        {

        }

        public SymbolTokenValue Value { get; }

        public override ParseResult Process(ITokenStream stream, IParsingContext context)
        {
            var token = stream.GetCurrentToken();

            var parseResult = ParseResult.Fail;

            if (
                token is SymbolToken symbolToken &&
                this.Value == symbolToken.Value)
            {
                this.Processor(token, context);

                parseResult = ParseResult.Success;
                stream.AdvanceStreamPosition();
            }

            return parseResult;
        }
    }
}