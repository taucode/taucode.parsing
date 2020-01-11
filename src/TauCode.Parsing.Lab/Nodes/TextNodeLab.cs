using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Lab.Tokens;
using TauCode.Parsing.Nodes;

namespace TauCode.Parsing.Lab.Nodes
{
    public class TextNodeLab : ActionNode
    {
        private readonly HashSet<ITextClassLab> _textClasses;

        public TextNodeLab(
            IEnumerable<ITextClassLab> textClasses,
            Action<ActionNode, IToken, IResultAccumulator> action,
            INodeFamily family,
            string name)
            : base(action, family, name)
        {
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

        public TextNodeLab(
            ITextClassLab textClass,
            Action<ActionNode, IToken, IResultAccumulator> action,
            INodeFamily family,
            string name)
            : this(new[] { textClass }, action, family, name)
        {
        }

        protected override InquireResult InquireImpl(IToken token, IResultAccumulator resultAccumulator)
        {
            var acceptsToken =
                token is TextTokenLab textToken &&
                _textClasses.Contains(textToken.Class);

            if (acceptsToken)
            {
                return this.Action == null ? InquireResult.Skip : InquireResult.Act;
            }
            else
            {
                return InquireResult.Reject;
            }
        }
    }
}
