using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Building
{
    // todo: deal with empty blocks, alt-s, opt-s and seq-s, which is an error.
    public class Builder : IBuilder
    {
        private class NodeBox
        {
            private readonly INode _node;
            private readonly List<string> _links;
            private bool _linksRequested;

            public NodeBox(INode node, IEnumerable<string> links = null)
            {
                // todo checks
                _node = node ?? throw new ArgumentNullException(nameof(node));
                _links = (links ?? new List<string>()).ToList();
            }

            public INode GetNode() => _node;
            public IReadOnlyList<string> Links => _links;

            public void RequestLink(NodeBox to)
            {
                // todo checks

                if (_linksRequested)
                {
                    throw new NotImplementedException(); // error.
                }

                if (_links.Any())
                {
                    foreach (var link in _links)
                    {
                        if (link == "NEXT")
                        {
                            _node.EstablishLink(to._node);
                        }
                        else
                        {
                            _node.ClaimLink(link);
                        }
                    }
                }
                else
                {
                    _node.EstablishLink(to._node);
                }

                _linksRequested = true;
            }

            public void DemandLink(NodeBox to)
            {
                if (_linksRequested)
                {
                    throw new NotImplementedException();
                }

                if (_links.Any())
                {
                    throw new NotImplementedException(); // you may not demand link if there are '_links'
                }

                _node.EstablishLink(to._node);
            }
        }

        private class BuildResult
        {
            public BuildResult(NodeBox head, NodeBox tail)
            {
                this.Head = head ?? throw new ArgumentNullException(nameof(head));
                this.Tail = tail ?? throw new ArgumentNullException(nameof(tail));
            }

            public NodeBox Head { get; }
            public NodeBox Tail { get; }
        }

        private Dictionary<string, PseudoList> _defblocks;
        private INodeFamily _family;

        public INode Build(PseudoList defblocks)
        {
            // todo: checks.
            _defblocks = defblocks.ToDictionary(
                x => x.GetSingleKeywordArgument<Symbol>(":name").Name,
                x => x.AsPseudoList());

            _family = new NodeFamily("todo lispush");

            var topBlock = _defblocks
                .Values
                .Single(x => x.GetSingleArgumentAsBool(":is-top") == true);

            var topBlockContent = topBlock.GetFreeArguments();

            var result = this.BuildContent(topBlockContent);

            return result.Head.GetNode();
        }

        private BuildResult BuildContent(PseudoList content)
        {
            NodeBox head = null;
            NodeBox tail = null;

            foreach (var item in content)
            {
                var result = this.BuildItem(item);

                if (head == null)
                {
                    // first entry

                    head = result.Head;
                    tail = result.Tail;
                }
                else
                {
                    tail.RequestLink(result.Head);
                    tail = result.Tail;
                }
            }

            // todo: check for null (which means empty 'sequence', actually).

            if (tail.Links.Any())
            {
                throw new NotImplementedException();
            }

            var buildResult = new BuildResult(head, tail);

            return buildResult;
        }

        private BuildResult BuildItem(Element item)
        {
            var car = item.GetCarSymbolName();
            BuildResult buildResult;

            switch (car)
            {
                case "BLOCK":
                    buildResult = this.BuildBlock(item);
                    break;

                case "ALT":
                    buildResult = this.BuildAlt(item);
                    break;

                case "OPT":
                    buildResult = this.BuildOpt(item);
                    break;

                case "SEQ":
                    buildResult = this.BuildSeq(item);
                    break;

                default:
                    buildResult = this.BuildCustomItem(item);
                    break;
            }

            return buildResult;
        }

        private static string GetItemName(Element item)
        {
            return item.GetSingleKeywordArgument<Symbol>(":name", true)?.Name;
        }

        private BuildResult BuildCustomItem(Element item)
        {
            var car = item.GetCarSymbolName();
            var links = this.GetItemLinks(item);

            INode node;

            switch (car)
            {
                case "WORD":
                    node = new ExactWordNode(
                        item.GetSingleKeywordArgument<StringAtom>(":value").Value,
                        null,
                        _family,
                        GetItemName(item));
                    break;

                case "SOME-IDENT":
                    node = new IdentifierNode(
                        null,
                        _family,
                        GetItemName(item));
                    break;

                case "SOME-WORD":
                    node = new WordNode(
                        null,
                        _family,
                        GetItemName(item));
                    break;

                // todo !!! need to implement nod factory asap :/
                //case "SYMBOL":
                //    node = new ExactSymbolNode(
                //        _family,
                //        GetItemName(item),
                //        null,
                //        item.GetSingleKeywordArgument<StringAtom>(":value").Value.Single());
                //    break;

                case "SOME-INT":
                    node = new IntegerNode(
                        null,
                        _family,
                        GetItemName(item));
                    break;

                case "SOME-STRING":
                    node = new StringNode(
                        null,
                        _family,
                        GetItemName(item));
                    break;

                case "IDLE": // todo: to std items, out of custom?
                    node = new IdleNode(
                        _family,
                        GetItemName(item));
                    break;

                case "END": // todo: to std items, out of custom?
                    node = EndNode.Instance;
                    break;

                default:
                    throw new NotImplementedException();
            }

            var nodeBox = new NodeBox(node, links);
            return new BuildResult(nodeBox, nodeBox);
        }

        private List<string> GetItemLinks(Element item)
        {
            var links = item
                .GetAllKeywordArguments(":links", true)
                .Select(x => x.AsElement<Symbol>().Name)
                .ToList();

            return links;
        }

        private BuildResult BuildBlock(Element item)
        {
            var blockName = item.GetSingleKeywordArgument<Symbol>(":ref").Name;
            var defblock = _defblocks[blockName];
            var args = defblock.GetFreeArguments();

            var blockEnter = new NodeBox(new IdleNode(_family, blockName));
            var contentResult = this.BuildContent(args);

            blockEnter.DemandLink(contentResult.Head);
            var result = new BuildResult(blockEnter, contentResult.Tail);

            return result;
        }

        private BuildResult BuildAlt(Element item)
        {
            var alternatives = item.GetFreeArguments();

            var altEnter = new NodeBox(new IdleNode(_family, GetItemName(item)));
            var altExit = new NodeBox(new IdleNode(_family, null));

            foreach (var alternative in alternatives)
            {
                var alternativeResult = this.BuildItem(alternative);

                altEnter.DemandLink(alternativeResult.Head);
                alternativeResult.Tail.RequestLink(altExit);
            }

            var result = new BuildResult(altEnter, altExit);

            return result;
        }

        private BuildResult BuildOpt(Element item)
        {
            var optEnter = new NodeBox(new IdleNode(_family, GetItemName(item)));
            var optExit = new NodeBox(new IdleNode(_family, null));

            // short circuit!
            optEnter.DemandLink(optExit);

            var args = item.GetFreeArguments();
            var contentResult = this.BuildContent(args);

            optEnter.DemandLink(contentResult.Head);
            contentResult.Tail.RequestLink(optExit);

            var result = new BuildResult(optEnter, optExit);

            return result;
        }

        private BuildResult BuildSeq(Element item)
        {
            var args = item.GetFreeArguments();
            var result = this.BuildContent(args);
            return result;
        }
    }
}
