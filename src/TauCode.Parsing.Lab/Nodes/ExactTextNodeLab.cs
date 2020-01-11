using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Lab.Tokens;
using TauCode.Parsing.Nodes;

namespace TauCode.Parsing.Lab.Nodes
{
    public class ExactTextNodeLab : ActionNode
    {
        private readonly HashSet<ITextClassLab> _textClasses;

        public ExactTextNodeLab(
            string exactText,
            IEnumerable<ITextClassLab> textClasses,
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

            _textClasses = new HashSet<ITextClassLab>(textClassesList);

            this.IsCaseSensitive = isCaseSensitive;
        }

        public ExactTextNodeLab(
            string exactText,
            ITextClassLab textClass,
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

        protected override InquireResult InquireImpl(IToken token, IResultAccumulator resultAccumulator)
        {
            if (token is TextTokenLab textToken)
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
                        return this.Action == null ? InquireResult.Skip : InquireResult.Act;
                    }
                }
            }

            return InquireResult.Reject;

            // todo clean
            //var acceptsToken =
            //    token is TextTokenLab textToken &&
            //    _textClasses.Contains(textToken.Class) &&
            //    string.Equals(
            //        textToken.Text,
            //        this.ExactText,
            //        this.IsCaseSensitive ? StringComparison.InvariantCulture : StringComparison.InvariantCultureIgnoreCase);

            //if (acceptsToken)
            //{
            //    return this.Action == null ? InquireResult.Skip : InquireResult.Act;
            //}
            //else
            //{
            //    return InquireResult.Reject;
            //}
        }

        public string ExactText { get; }

        public bool IsCaseSensitive { get; }
    }
}
