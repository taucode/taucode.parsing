using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Nodes
{
    public class ExactTextNode : ActionNode
    {
        private readonly HashSet<ITextClass> _textClasses;

        public ExactTextNode(
            string exactText,
            IEnumerable<ITextClass> textClasses,
            bool isCaseSensitive,
            Action<ActionNode, IToken, IResultAccumulator> action,
            INodeFamily family,
            string name)
            : base(action, family, name)
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

            var textClassesList = textClasses.ToList();
            if (textClassesList.Count == 0)
            {
                throw new ArgumentException($"'{nameof(textClasses)}' cannot be empty.");
            }

            if (textClassesList.Any(x => x == null))
            {
                throw new ArgumentException($"'{nameof(textClasses)}' cannot contain nulls.");
            }

            _textClasses = new HashSet<ITextClass>(textClassesList);

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

        protected override bool InquireImpl(IToken token, IResultAccumulator resultAccumulator)
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
                        _textClasses.Contains(textTokenClass) ||
                        _textClasses.Any(x => string.Equals(text, x.TryConvertFrom(text, textTokenClass)))
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
