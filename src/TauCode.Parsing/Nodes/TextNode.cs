using System;
using System.Collections.Generic;
using TauCode.Extensions;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Nodes
{
    public class TextNode : ActionNode
    {
        private readonly ITextClass[] _textClasses;

        public TextNode(
            IEnumerable<ITextClass> textClasses,
            Action<IToken, IResultAccumulator> action,
            INodeFamily family,
            string name)
            : base(action, family, name)
        {
            // todo: check args
            _textClasses = new List<ITextClass>(textClasses).ToArray(); // todo optimize
        }

        public TextNode(
            ITextClass textClass,
            Action<IToken, IResultAccumulator> action,
            INodeFamily family,
            string name)
            : this(new[] { textClass }, action, family, name)
        {
        }

        protected override InquireResult InquireImpl(IToken token, IResultAccumulator resultAccumulator)
        {
            bool acceptsToken =
                token is TextToken textToken &&
                textToken.Class.IsIn(_textClasses);

            if (acceptsToken)
            {
                return this.Action == null ? InquireResult.Skip : InquireResult.Act;
            }
            else
            {
                return InquireResult.Reject;
            }
        }

        public IReadOnlyList<ITextClass> TextClasses => _textClasses;
    }
}
