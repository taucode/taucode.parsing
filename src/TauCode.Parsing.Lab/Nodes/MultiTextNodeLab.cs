using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Lab.Tokens;
using TauCode.Parsing.Nodes;

namespace TauCode.Parsing.Lab.Nodes
{
    public class MultiTextNodeLab : ActionNode
    {
        private readonly List<string> _texts;
        private readonly HashSet<ITextClassLab> _textClasses;

        public MultiTextNodeLab(
            IEnumerable<string> texts,
            IEnumerable<ITextClassLab> textClasses,
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

            _textClasses = new HashSet<ITextClassLab>(textClassesList);
        }

        protected override InquireResult InquireImpl(IToken token, IResultAccumulator resultAccumulator)
        {
            if (token is TextTokenLab textToken && _textClasses.Contains(textToken.Class))
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
