using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Extensions;
using TauCode.Parsing.Building;
using TauCode.Parsing.Lab.Nodes;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Lab.Building
{
    public class NodeFactoryBaseLab : INodeFactory
    {
        private readonly IDictionary<string, ITextClass> _textClasses;
        private readonly bool _isCaseSensitive;

        protected NodeFactoryBaseLab(
            string nodeFamilyName,
            IList<ITextClass> textClasses,
            bool isCaseSensitive)
        {
            this.NodeFamily = new NodeFamily(nodeFamilyName);
            textClasses = textClasses ?? new List<ITextClass>();

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
            // todo: wrap it into try/catch => throw bad_grammar

            var car = item.GetCarSymbolName();
            INode node;

            switch (car)
            {
                case "EXACT-TEXT":
                    node = new ExactTextNodeLab(
                        item.GetSingleKeywordArgument<StringAtom>(":value").Value,
                        this.ParseTextClasses(item.GetAllKeywordArguments(":classes")),
                        _isCaseSensitive,
                        null,
                        this.NodeFamily,
                        item.GetItemName());
                    break;

                case "SOME-TEXT":
                    node = new TextNodeLab(
                        this.ParseTextClasses(item.GetAllKeywordArguments(":classes")),
                        null,
                        this.NodeFamily,
                        item.GetItemName());
                    break;

                case "MULTI-TEXT":
                    node = new MultiTextNodeLab(
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

                default:
                    throw new NotImplementedException();
            }

            return node;
        }

        private IEnumerable<ITextClass> ParseTextClasses(PseudoList arguments)
        {
            var textClasses = new List<ITextClass>();

            foreach (var argument in arguments)
            {
                var symbolElement = (Symbol)argument;
                var textClass = this.ResolveTextClass(symbolElement.Name);

                // todo clean
                //switch (symbolElement.Name)
                //{
                //    case "WORD":
                //        textClass = WordTextClassLab.Instance;
                //        break;

                //    case "IDENTIFIER":
                //        textClass = SqlIdentifierClass.Instance;
                //        break;

                //    case "STRING":
                //        textClass = StringTextClassLab.Instance;
                //        break;

                //    default:
                //        throw new ArgumentOutOfRangeException();
                //}

                textClasses.Add(textClass);
            }

            return textClasses;
        }

        protected virtual ITextClass ResolveTextClass(string tag)
        {
            var textClass = _textClasses.GetOrDefault(tag.ToLowerInvariant());
            if (textClass == null)
            {
                throw new NotImplementedException(); // cannot resolve
            }

            return textClass;
        }
    }
}
