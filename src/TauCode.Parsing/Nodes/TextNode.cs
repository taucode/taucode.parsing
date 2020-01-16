using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Nodes
{
    // todo clean
    public class TextNode : ActionNode
    {
        private readonly HashSet<ITextClass> _textClasses;

        public TextNode(
            IEnumerable<ITextClass> textClasses,
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

            _textClasses = new HashSet<ITextClass>(textClassesList);
        }

        public TextNode(
            ITextClass textClass,
            Action<ActionNode, IToken, IResultAccumulator> action,
            INodeFamily family,
            string name)
            : this(new[] { textClass }, action, family, name)
        {
        }

        protected override /*InquireResult*/ bool InquireImpl(IToken token, IResultAccumulator resultAccumulator)
        {
            if (token is TextToken textToken)
            {
                var text = textToken.Text;

                var textTokenClass = textToken.Class;
                if (
                    _textClasses.Contains(textTokenClass) ||
                    _textClasses.Any(x => x.TryConvertFrom(text, textTokenClass) != null)
                )
                {
                    return true;
                    //return this.Action == null ? InquireResult.Skip : InquireResult.Act;
                }
            }

            return false;
            //return InquireResult.Reject;
        }
    }
}
