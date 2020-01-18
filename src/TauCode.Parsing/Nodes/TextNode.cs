using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Nodes
{
    public class TextNode : TextNodeBase
    {
        public TextNode(
            IEnumerable<ITextClass> textClasses,
            Action<ActionNode, IToken, IResultAccumulator> action,
            INodeFamily family,
            string name)
            : base(
                textClasses,
                action,
                family,
                name)
        {
        }

        public TextNode(
            ITextClass textClass,
            Action<ActionNode, IToken, IResultAccumulator> action,
            INodeFamily family,
            string name)
            : base(
                textClass,
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

                var textTokenClass = textToken.Class;
                if (
                    this.TextClassesImpl.Contains(textTokenClass) ||
                    this.TextClassesImpl.Any(x => x.TryConvertFrom(text, textTokenClass) != null)
                )
                {
                    return true;
                }
            }

            return false;
        }
    }
}
