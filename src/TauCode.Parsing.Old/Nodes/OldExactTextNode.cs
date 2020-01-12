using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.Old.Tokens;

namespace TauCode.Parsing.Old.Nodes
{
    public class OldExactTextNode : ActionNode
    {
        private readonly HashSet<IOldTextClass> _textClasses;

        public OldExactTextNode(
            string exactText,
            IEnumerable<IOldTextClass> textClasses,
            Action<ActionNode, IToken, IResultAccumulator> action,
            INodeFamily family,
            string name)
            : base(action, family, name)
        {
            this.ExactText = exactText ?? throw new ArgumentNullException(nameof(exactText));

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

            _textClasses = new HashSet<IOldTextClass>(textClassesList);

        }

        public OldExactTextNode(
            string exactText,
            IOldTextClass textClass,
            Action<ActionNode, IToken, IResultAccumulator> action,
            INodeFamily family,
            string name)
            : this(exactText, new[] { textClass }, action, family, name)
        {
        }

        protected override InquireResult InquireImpl(IToken token, IResultAccumulator resultAccumulator)
        {
            var acceptsToken =
                token is OldTextToken textToken &&
                _textClasses.Contains(textToken.Class) &&
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

        public bool IsCaseSensitive { get; set; }
    }
}
