using System;
using System.Collections.Generic;
using System.Linq;

namespace TauCode.Parsing.Nodes
{
    public abstract class TextNodeBase : ActionNode
    {
        protected HashSet<ITextClass> TextClassesImpl { get; set; }

        protected TextNodeBase(
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

            this.TextClassesImpl = new HashSet<ITextClass>(textClassesList);
            this.TextClasses = this.TextClassesImpl.ToList();
        }

        protected TextNodeBase(
            ITextClass textClass,
            Action<ActionNode, IToken, IResultAccumulator> action,
            INodeFamily family,
            string name)
            : this(
                new[] { textClass },
                action,
                family,
                name)
        {
        }

        public IReadOnlyList<ITextClass> TextClasses { get; }
    }
}
