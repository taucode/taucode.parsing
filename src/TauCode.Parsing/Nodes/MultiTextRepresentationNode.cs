using System;
using System.Collections.Generic;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Nodes
{
    public class MultiTextRepresentationNode : ActionNode
    {
        private readonly HashSet<string> _textRepresentationVariants;
        private readonly HashSet<ITextClass> _textClasses;

        public MultiTextRepresentationNode(
            IEnumerable<string> textRepresentationVariants,
            IEnumerable<ITextClass> textClasses,
            Func<TextToken, string> textTokenRepresentationGetter,
            Action<ActionNode, IToken, IResultAccumulator> action,
            INodeFamily family,
            string name)
            : base(action, family, name)
        {
            if (textRepresentationVariants == null)
            {
                _textRepresentationVariants = null;
            }
            else
            {
                _textRepresentationVariants = new HashSet<string>(textRepresentationVariants);
            }

            if (textClasses == null)
            {
                // todo: also check collection for nulls.
                throw new ArgumentNullException(nameof(textClasses));
            }

            _textClasses = new HashSet<ITextClass>(textClasses);

            this.TextTokenRepresentationGetter =
                textTokenRepresentationGetter ?? 
                throw new ArgumentNullException(nameof(textTokenRepresentationGetter));
        }

        protected override InquireResult InquireImpl(IToken token, IResultAccumulator resultAccumulator)
        {
            if (token is TextToken textToken && _textClasses.Contains(textToken.Class))
            {
                if (_textRepresentationVariants == null)
                {
                    // any representation does.
                    return this.Action == null ? InquireResult.Skip : InquireResult.Act;
                }
                else
                {
                    var representation = this.TextTokenRepresentationGetter(textToken);
                    if (_textRepresentationVariants.Contains(representation))
                    {
                        return this.Action == null ? InquireResult.Skip : InquireResult.Act;
                    }
                }
            }

            return InquireResult.Reject;
        }

        public Func<TextToken, string> TextTokenRepresentationGetter;
    }
}
