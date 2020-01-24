using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Nodes
{
    public class MultiTextNode : TextNodeBase
    {
        private readonly HashSet<string> _texts;

        public MultiTextNode(
            IEnumerable<string> texts,
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
            if (texts == null)
            {
                throw new ArgumentNullException(nameof(texts));
            }

            var textList = texts.ToList();

            if (textList.Count == 0)
            {
                throw new ArgumentException($"'{nameof(texts)}' cannot be empty.", nameof(texts));
            }

            if (textList.Any(x => x == null))
            {
                throw new ArgumentException($"'{nameof(texts)}' cannot contain nulls.", nameof(texts));
            }

            if (!isCaseSensitive)
            {
                textList = textList.Select(x => x.ToLowerInvariant()).ToList();
            }

            _texts = new HashSet<string>(textList);

            this.IsCaseSensitive = isCaseSensitive;
            this.Texts = _texts.ToList();
        }

        public MultiTextNode(
            IEnumerable<string> texts,
            ITextClass textClass,
            bool isCaseSensitive,
            Action<ActionNode, IToken, IResultAccumulator> action,
            INodeFamily family,
            string name)
            : this(
                texts,
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

                if (_texts.Contains(text))
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

        public bool IsCaseSensitive { get; }

        public IReadOnlyList<string> Texts { get; }
    }
}
