using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Building
{
    public class NodeFactoryBase : INodeFactory
    {
        private readonly IDictionary<string, ITextClass> _textClasses;
        private readonly bool _isCaseSensitive;

        protected NodeFactoryBase(
            string nodeFamilyName,
            IList<ITextClass> textClasses,
            bool isCaseSensitive)
        {
            this.NodeFamily = new NodeFamily(nodeFamilyName);
            textClasses ??= new List<ITextClass>();

            _textClasses = new Dictionary<string, ITextClass>();

            foreach (var textClass in textClasses)
            {
                var tag = textClass.Tag?.ToLowerInvariant();
                if (tag == null || _textClasses.ContainsKey(tag))
                {
                    continue; // won't add it to the collection
                }

                _textClasses.Add(tag, textClass);
            }

            _isCaseSensitive = isCaseSensitive;
        }

        public INodeFamily NodeFamily { get; }

        public virtual INode CreateNode(PseudoList item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            try
            {
                var car = item.GetCarSymbolName();
                INode node;

                switch (car)
                {
                    case "EXACT-TEXT":
                        node = new ExactTextNode(
                            item.GetSingleKeywordArgument<StringAtom>(":value").Value,
                            this.ParseTextClasses(item.GetAllKeywordArguments(":classes")),
                            _isCaseSensitive,
                            null,
                            this.NodeFamily,
                            item.GetItemName());
                        break;

                    case "SOME-TEXT":
                        node = new TextNode(
                            this.ParseTextClasses(item.GetAllKeywordArguments(":classes")),
                            null,
                            this.NodeFamily,
                            item.GetItemName());
                        break;

                    case "MULTI-TEXT":
                        node = new MultiTextNode(
                            item.GetAllKeywordArguments(":values").Cast<StringAtom>().Select(x => x.Value),
                            this.ParseTextClasses(item.GetAllKeywordArguments(":classes")),
                            _isCaseSensitive,
                            null,
                            this.NodeFamily,
                            item.GetItemName());
                        break;

                    case "EXACT-PUNCTUATION":
                        node = new ExactPunctuationNode(
                            item.GetSingleKeywordArgument<StringAtom>(":value").Value.Single(),
                            null,
                            this.NodeFamily,
                            item.GetItemName());
                        break;

                    case "SOME-INTEGER":
                        node = new IntegerNode(
                            null,
                            this.NodeFamily,
                            item.GetItemName());
                        break;

                    case "FALLBACK":
                        var name = item.GetItemName();
                        if (name == null)
                        {
                            throw new BuildingException("Fallback node must have a name.");
                        }

                        node = new FallbackNode(
                            this.CreateFallbackPredicate(name),
                            this.NodeFamily,
                            name);

                        break;

                    default:
                        return null;
                }

                return node;
            }
            catch (Exception ex)
            {
                throw new BuildingException($"Could not build a node from item {item}.", ex);
            }
        }

        protected virtual Func<FallbackNode, IToken, IResultAccumulator, bool> CreateFallbackPredicate(string nodeName)
        {
            throw new NotSupportedException($"Override '{nameof(CreateFallbackPredicate)}' if you need support of fallback nodes.");
        }

        private IEnumerable<ITextClass> ParseTextClasses(PseudoList arguments)
        {
            var textClasses = new List<ITextClass>();

            foreach (var argument in arguments)
            {
                var symbolElement = (Symbol)argument;
                var textClass = this.ResolveTextClass(symbolElement.Name);
                textClasses.Add(textClass);
            }

            return textClasses;
        }

        protected virtual ITextClass ResolveTextClass(string tag)
        {
            if (tag == null)
            {
                throw new ArgumentNullException(nameof(tag));
            }

            var textClass = _textClasses.GetOrDefault(tag.ToLowerInvariant());
            if (textClass == null)
            {
                throw new BuildingException($"Could not resolve text class with tag'{tag}'.");
            }

            return textClass;
        }
    }
}
