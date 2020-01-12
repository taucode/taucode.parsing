using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Building
{
    public class TreeBuilder : ITreeBuilder
    {
        #region Fields

        private Dictionary<string, PseudoList> _defblocks;
        private INodeFactory _nodeFactory;

        private int _nextUnnamedOptIndex;
        private int _nextUnnamedAltIndex;

        #endregion

        #region Constructor



        #endregion

        #region Private

        private NodeBunch BuildContent(PseudoList content)
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

            if (tail == null)
            {
                throw new BuildingException("Content is empty.");
            }

            if (tail.Links.Any())
            {
                throw new BuildingException("Last item in a content must not have explicit links.");
            }

            var buildResult = new NodeBunch(head, tail);

            return buildResult;
        }

        private NodeBunch BuildItem(Element item)
        {
            var car = item.GetCarSymbolName();
            NodeBunch buildResult;

            INode node;
            NodeBox nodeBox;
            
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

                case "IDLE":
                    node = new IdleNode(
                        _nodeFactory.NodeFamily,
                        item.GetItemName());
                    nodeBox = new NodeBox(node, item.GetItemLinks());
                    buildResult = new NodeBunch(nodeBox, nodeBox);
                    break;

                case "END":
                    node = EndNode.Instance;
                    nodeBox = new NodeBox(node);
                    buildResult = new NodeBunch(nodeBox, nodeBox);
                    break;

                default:
                    buildResult = this.BuildCustomItem(item);
                    break;
            }

            return buildResult;
        }

        private NodeBunch BuildCustomItem(Element item)
        {
            var links = item.GetItemLinks();

            var headNode = _nodeFactory.CreateNode(item.AsPseudoList());

            if (headNode == null)
            {
                throw new BuildingException("Node factory returned null.");
            }

            var tree = headNode.FetchTree();
            if (tree.Count == 1)
            {
                var nodeBox = new NodeBox(headNode, links);
                return new NodeBunch(nodeBox, nodeBox);
            }
            else
            {
                var headNodeBox = new NodeBox(headNode);

                try
                {
                    var tailNode = tree.Single(x => x.EstablishedLinks.Count == 0);
                    var tailNodeBox = new NodeBox(tailNode, links);

                    return new NodeBunch(headNodeBox, tailNodeBox);

                }
                catch (Exception ex)
                {
                    throw new BuildingException("More than one tail node (the node which doesn't have links).", ex);
                }
            }
        }

        private NodeBunch BuildBlock(Element item)
        {
            var blockName = item.GetSingleKeywordArgument<Symbol>(":ref").Name;
            var defblock = _defblocks[blockName];
            var args = defblock.GetFreeArguments();

            var blockEnter = new NodeBox(new IdleNode(_nodeFactory.NodeFamily, blockName));
            var contentResult = this.BuildContent(args);
            var blockExit = new NodeBox(new IdleNode(_nodeFactory.NodeFamily, $"<exit of block> {blockName}"), item.GetItemLinks());

            blockEnter.DemandLink(contentResult.Head);
            contentResult.Tail.RequestLink(blockExit);

            var result = new NodeBunch(blockEnter, blockExit);

            return result;
        }

        private NodeBunch BuildAlt(Element item)
        {
            var altName = item.GetItemName() ?? this.GetNextAltName();
            var alternatives = item.GetFreeArguments();

            var altEnter = new NodeBox(new IdleNode(_nodeFactory.NodeFamily, altName));
            var altExit = new NodeBox(new IdleNode(_nodeFactory.NodeFamily, $"<exit of alt> {altName}"), item.GetItemLinks());

            foreach (var alternative in alternatives)
            {
                var alternativeResult = this.BuildItem(alternative);

                altEnter.DemandLink(alternativeResult.Head);
                alternativeResult.Tail.RequestLink(altExit);
            }

            var result = new NodeBunch(altEnter, altExit);

            return result;
        }

        private NodeBunch BuildOpt(Element item)
        {
            var optName = item.GetItemName() ?? this.GetNextOptName();

            var optEnter = new NodeBox(new IdleNode(_nodeFactory.NodeFamily, optName));
            var optExit = new NodeBox(new IdleNode(_nodeFactory.NodeFamily, $"<exit of opt> {optName}"), item.GetItemLinks());

            // short circuit!
            optEnter.DemandLink(optExit);

            var args = item.GetFreeArguments();
            var contentResult = this.BuildContent(args);

            optEnter.DemandLink(contentResult.Head);
            contentResult.Tail.RequestLink(optExit);

            var result = new NodeBunch(optEnter, optExit);

            return result;
        }

        private NodeBunch BuildSeq(Element item)
        {
            var args = item.GetFreeArguments();
            var result = this.BuildContent(args);
            return result;
        }

        private string GetNextOptName()
        {
            var name = $"<unnamed opt #{_nextUnnamedOptIndex}>";
            _nextUnnamedOptIndex++;
            return name;
        }

        private string GetNextAltName()
        {
            var name = $"<unnamed alt #{_nextUnnamedAltIndex}>";
            _nextUnnamedAltIndex++;
            return name;
        }

        #endregion

        #region IBuilder Members

        public INode Build(INodeFactory nodeFactory, PseudoList defblocks)
        {
            if (defblocks == null)
            {
                throw new ArgumentNullException(nameof(defblocks));
            }

            _nodeFactory = nodeFactory ?? throw new ArgumentNullException(nameof(nodeFactory));

            _defblocks = defblocks.ToDictionary(
                x => x.GetSingleKeywordArgument<Symbol>(":name").Name,
                x => x.AsPseudoList());

            var topBlocks = _defblocks
                .Values
                .Where(x => x.GetSingleArgumentAsBool(":is-top") == true)
                .ToList();

            if (topBlocks.Count == 0)
            {
                throw new BuildingException("No top defblocks defined.");
            }

            if (topBlocks.Count > 1)
            {
                throw new BuildingException("More than one top defblock defined.");
            }

            var topBlock = topBlocks[0];
            var topBlockContent = topBlock.GetFreeArguments();
            var result = this.BuildContent(topBlockContent);
            return result.Head.GetNode();
        }

        #endregion
    }
}
