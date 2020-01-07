using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Nodes
{
    public class MultiTextNode : ActionNode
    {
        private readonly List<string> _texts;
        private readonly HashSet<ITextClass> _textClasses;

        public MultiTextNode(
            IEnumerable<string> texts,
            IEnumerable<ITextClass> textClasses,
            Action<ActionNode, IToken, IResultAccumulator> action,
            INodeFamily family,
            string name)
            : base(action, family, name)
        {
            if (texts == null)
            {
                throw new ArgumentNullException(nameof(texts));
            }

            _texts = texts.ToList();
            if (_texts.Count == 0)
            {
                throw new ArgumentException($"'{nameof(texts)}' cannot be empty.", nameof(texts));
            }

            if (_texts.Any(x => x == null))
            {
                throw new ArgumentException($"'{nameof(texts)}' cannot contain nulls.", nameof(texts));
            }

            if (textClasses == null)
            {
                throw new ArgumentNullException(nameof(textClasses));
            }

            var textClassesList = textClasses.ToList(); // to avoid multiple enumerating.

            if (textClassesList.Count == 0)
            {
                throw new ArgumentException($"'{nameof(textClasses)}' cannot be empty.");
            }

            if (textClassesList.Any(x => x == null))
            {
                throw new ArgumentException($"'{nameof(textClasses)}' cannot contain nulls.");
            }

            _textClasses = new HashSet<ITextClass>(textClassesList);
        }

        protected override InquireResult InquireImpl(IToken token, IResultAccumulator resultAccumulator)
        {
            if (token is TextToken textToken && _textClasses.Contains(textToken.Class))
            {
                foreach (var text in _texts)
                {
                    if (string.Equals(
                        textToken.Text,
                        text,
                        this.IsCaseSensitive
                            ? StringComparison.InvariantCulture
                            : StringComparison.InvariantCultureIgnoreCase))
                    {
                        return this.Action == null ? InquireResult.Skip : InquireResult.Act;
                    }
                }
            }

            return InquireResult.Reject;
        }

        public bool IsCaseSensitive { get; set; } = false;
    }
}
