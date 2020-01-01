using System;
using System.Collections.Generic;
using TauCode.Extensions;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Nodes
{
    public class ExactTextNode : ActionNode
    {
        private readonly ITextClass[] _textClasses;

        public ExactTextNode(
            string exactText,
            IEnumerable<ITextClass> textClasses,
            Action<IToken, IResultAccumulator> action,
            INodeFamily family,
            string name)
            : base(action, family, name)
        {
            this.ExactText = exactText ?? throw new ArgumentNullException(nameof(exactText));
            // todo: check args
            _textClasses = new List<ITextClass>(textClasses).ToArray(); // todo optimize

        }

        public ExactTextNode(
            string exactText,
            ITextClass textClass,
            Action<IToken, IResultAccumulator> action,
            INodeFamily family,
            string name)
            : this(exactText, new[] { textClass }, action, family, name)
        {
        }

        protected override InquireResult InquireImpl(IToken token, IResultAccumulator resultAccumulator)
        {
            bool acceptsToken =
                token is TextToken textToken &&
                textToken.Class.IsIn(_textClasses) &&
                string.Equals(
                    textToken.Text,
                    this.ExactText,
                    this.IsCaseSensitive ? StringComparison.InvariantCulture : StringComparison.InvariantCultureIgnoreCase);

            if (acceptsToken)
            {
                return this.Action == null ? InquireResult.Skip : InquireResult.Act;
            }
            else
            {
                return InquireResult.Reject;
            }
        }

        public string ExactText { get; }

        public IReadOnlyList<ITextClass> TextClasses => _textClasses;

        public bool IsCaseSensitive { get; set; }
    }
}
