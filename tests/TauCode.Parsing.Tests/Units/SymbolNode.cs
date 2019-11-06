using System;
using System.Diagnostics;
using TauCode.Parsing.ParsingUnits.Impl;
using TauCode.Parsing.Tests.Tokens;

namespace TauCode.Parsing.Tests.Units
{
    [DebuggerDisplay("{" + nameof(Value) + "}")]
    public class SymbolNode : ParsingNode
    {
        public SymbolNode(SymbolValue value, Action<IToken, IParsingContext> processor)
            : base(processor)
        {
            this.Value = value;
        }

        public SymbolNode(char c, Action<IToken, IParsingContext> processor)
            : this(Helper.SymbolTokenFromChar(c), processor)
        {

        }

        public SymbolValue Value { get; }

        //public override IReadOnlyList<IParsingUnit> Process(ITokenStream stream, IParsingContext context)
        //{
        //    var token = stream.GetCurrentToken();

        //    if (
        //        token is SymbolToken symbolToken &&
        //        this.Value == symbolToken.Value)
        //    {
        //        this.Processor(token, context);
        //        stream.AdvanceStreamPosition();

        //        return this.NextUnits;
        //    }

        //    return null;
        //}

        protected override bool IsAcceptableToken(IToken token)
        {
            return
                token is SymbolToken symbolToken &&
                this.Value == symbolToken.Value;
        }
    }
}

