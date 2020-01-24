using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Nodes
{
    public class ExactTextNode : TextNodeBase
    {
        public ExactTextNode(
            string exactText,
            IEnumerable<ITextClass> textClasses,
            bool isCaseSensitive,
            Action<ActionNode, IToken, IResultAccumulator> action,
            INodeFamily family,
            string name)
            : base(
                textClasses,
                action,
                family,
                name)
        {
            if (exactText == null)
            {
                throw new ArgumentNullException(nameof(exactText));
            }

            if (!isCaseSensitive)
            {
                exactText = exactText.ToLowerInvariant();
            }

            this.ExactText = exactText;

            if (textClasses == null)
            {
                throw new ArgumentNullException(nameof(textClasses));
            }

            this.IsCaseSensitive = isCaseSensitive;
        }

        public ExactTextNode(
            string exactText,
            ITextClass textClass,
            bool isCaseSensitive,
            Action<ActionNode, IToken, IResultAccumulator> action,
            INodeFamily family,
            string name)
            : this(
                exactText,
                new[] { textClass },
                isCaseSensitive,
                action,
                family,
                name)
        {
        }

        protected override bool AcceptsTokenImpl(IToken token, IResultAccumulator resultAccumulator)
        {
            if (token is TextToken textToken)
            {
                var text = textToken.Text;
                if (!this.IsCaseSensitive)
                {
                    text = text.ToLowerInvariant();
                }

                if (string.Equals(text, this.ExactText))
                {
                    var textTokenClass = textToken.Class;
                    if (
                        this.TextClassesImpl.Contains(textTokenClass) ||
                        this.TextClassesImpl.Any(x => string.Equals(text, x.TryConvertFrom(text, textTokenClass)))
                    )
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public string ExactText { get; }

        public bool IsCaseSensitive { get; }
    }
}
