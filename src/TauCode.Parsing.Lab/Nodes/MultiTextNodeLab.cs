using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Lab.Tokens;
using TauCode.Parsing.Nodes;

namespace TauCode.Parsing.Lab.Nodes
{
    public class MultiTextNodeLab : ActionNode
    {
        private readonly HashSet<string> _texts;
        private readonly HashSet<ITextClass> _textClasses;

        public MultiTextNodeLab(
            IEnumerable<string> texts,
            IEnumerable<ITextClass> textClasses,
            bool isCaseSensitive,
            Action<ActionNode, IToken, IResultAccumulator> action,
            INodeFamily family,
            string name)
            : base(action, family, name)
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

            this.IsCaseSensitive = isCaseSensitive;

            this.Texts = _texts.ToList();
        }

        protected override InquireResult InquireImpl(IToken token, IResultAccumulator resultAccumulator)
        {
            if (token is TextTokenLab textToken)
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
                        _textClasses.Contains(textTokenClass) ||
                        _textClasses.Any(x => string.Equals(text, x.TryConvertFrom(text, textTokenClass)))
                    )
                    {
                        return this.Action == null ? InquireResult.Skip : InquireResult.Act;
                    }
                }
            }

            // todo clean
            //if (
            //    token is TextTokenLab textToken &&
            //    (
            //        _textClasses.Contains(textToken.Class) ||
            //        _textClasses.Any(x => x.TryConvertFrom(textToken.Text, textToken.Class))
            //        )
            //    )
            //{
            //    foreach (var text in _texts)
            //    {
            //        if (string.Equals(
            //            textToken.Text,
            //            text,
            //            this.IsCaseSensitive
            //                ? StringComparison.InvariantCulture
            //                : StringComparison.InvariantCultureIgnoreCase))
            //        {
            //            return this.Action == null ? InquireResult.Skip : InquireResult.Act;
            //        }
            //    }
            //}

            return InquireResult.Reject;
        }

        public bool IsCaseSensitive { get; }

        public IReadOnlyList<string> Texts { get; }
    }
}
