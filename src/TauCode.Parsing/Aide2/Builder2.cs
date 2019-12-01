using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Aide2
{
    // todo: deal with empty blocks, which is an error.
    public class Builder2 : IBuilder2
    {
        private class NodeBox
        {
            public INode Node { get; set; }
            public List<string> Links { get; set; } = new List<string>();
        }

        private class BuildResult
        {
            public NodeBox Head { get; set; }
            public NodeBox Tail { get; set; }
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

            return result.Head.Node;
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
                    if (tail.Links.Any())
                    {
                        foreach (var link in tail.Links)
                        {
                            if (link == "NEXT")
                            {
                                tail.Node.EstablishLink(result.Head.Node);
                            }
                            else
                            {
                                tail.Node.ClaimLink(link);
                            }
                        }
                    }
                    else
                    {
                        tail.Node.EstablishLink(result.Head.Node);
                    }

                    tail = result.Tail;
                }
            }

            // todo: tail must contain "next" node link!

            var buildResult = new BuildResult
            {
                Head = head,
                Tail = tail,
            };

            return buildResult;
        }

        private BuildResult BuildItem(Element item)
        {
            var car = item.GetCarSymbolName();
            BuildResult buildResult;

            switch (car)
            {
                case "ALT":
                    buildResult = this.BuildAlt(item);
                    break;

                case "OPT":
                    buildResult = this.BuildOpt(item);
                    break;

                case "BLOCK":
                    buildResult = this.BuildBlock(item);
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

        private BuildResult BuildBlock(Element item)
        {
            var blockName = item.GetSingleKeywordArgument<Symbol>(":ref").Name;
            var defblock = _defblocks[blockName];
            var args = defblock.GetFreeArguments();

            var blockEnter = new NodeBox
            {
                Node = new IdleNode(_family, blockName),
            };

            var contentResult = this.BuildContent(args);

            blockEnter.Node.EstablishLink(contentResult.Head.Node);
            var result = new BuildResult
            {
                Head = blockEnter,
                Tail = contentResult.Tail,
            };

            return result;
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
                        _family,
                        GetItemName(item),
                        null,
                        item.GetSingleKeywordArgument<StringAtom>(":value").Value);
                    break;

                case "SOME-IDENT":
                    node = new IdentifierNode(
                        _family,
                        GetItemName(item),
                        null);
                    break;

                case "SOME-WORD":
                    node = new WordNode(
                        _family,
                        GetItemName(item),
                        null);
                    break;

                case "SYMBOL":
                    node = new ExactSymbolNode(
                        _family,
                        GetItemName(item),
                        null,
                        item.GetSingleKeywordArgument<StringAtom>(":value").Value.Single());
                    break;

                case "SOME-INT":
                    node = new IntegerNode(
                        _family,
                        GetItemName(item),
                        null);
                    break;

                case "SOME-STRING":
                    node = new StringNode(
                        _family,
                        GetItemName(item),
                        null);
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

            var nodeBox = new NodeBox
            {
                Node = node,
                Links = links,
            };

            return new BuildResult
            {
                Head = nodeBox,
                Tail = nodeBox,
            };
        }

        private List<string> GetItemLinks(Element item)
        {
            var links = item
                .GetAllKeywordArguments(":links", true)
                .Select(x => x.AsElement<Symbol>().Name)
                .ToList();

            return links;
        }

        private BuildResult BuildAlt(Element item)
        {
            var alternatives = item.GetFreeArguments();

            var altEnter = new NodeBox
            {
                Node = new IdleNode(_family, GetItemName(item)),
            };

            var altExit = new NodeBox
            {
                Node = new IdleNode(_family, null),
            };

            foreach (var alternative in alternatives)
            {
                var alternativeResult = this.BuildItem(alternative);

                altEnter.Node.EstablishLink(alternativeResult.Head.Node);
                alternativeResult.Tail.Node.EstablishLink(altExit.Node);
            }

            var result = new BuildResult
            {
                Head = altEnter,
                Tail = altExit,
            };

            return result;
        }

        private BuildResult BuildOpt(Element item)
        {
            var optEnter = new NodeBox
            {
                Node = new IdleNode(_family, GetItemName(item)),
            };

            var optExit = new NodeBox
            {
                Node = new IdleNode(_family, null),
            };

            // short circuit!
            optEnter.Node.EstablishLink(optExit.Node);

            var args = item.GetFreeArguments();
            var contentResult = this.BuildContent(args);

            optEnter.Node.EstablishLink(contentResult.Head.Node);
            contentResult.Tail.Node.EstablishLink(optExit.Node);

            var result = new BuildResult
            {
                Head = optEnter,
                Tail = optExit,
            };

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
