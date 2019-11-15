using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Nodes;

namespace TauCode.Parsing
{
    // todo clean up
    public static class ParsingHelper
    {
        public static bool IsEndOfStream(this ITokenStream tokenStream)
        {
            if (tokenStream == null)
            {
                throw new ArgumentNullException(nameof(tokenStream));
            }

            return tokenStream.Position == tokenStream.Tokens.Count;
        }

        public static void AdvanceStreamPosition(this ITokenStream tokenStream)
        {
            if (tokenStream == null)
            {
                throw new ArgumentNullException(nameof(tokenStream));
            }

            tokenStream.Position++;
        }

        public static IReadOnlyCollection<INode> GetNonIdleNodes(IReadOnlyCollection<INode> nodes) // todo: optimize. use IEnumerable?
        {
            if (nodes.Any(x => x is IdleNode))
            {
                var list = new List<INode>();

                foreach (var node in nodes)
                {
                    WriteNonIdleNodes(node, list);
                }

                return list;
            }
            else
            {
                return nodes;
            }
        }

        private static void WriteNonIdleNodes(INode node, List<INode> destination)
        {
            if (node is IdleNode)
            {
                var links = node.Links;
                foreach (var link in links)
                {
                    WriteNonIdleNodes(link, destination);
                }
            }
            else
            {
                destination.Add(node);
            }
        }

        public static void AddLinksByNames(this INode node, params string[] names)
        {
            // todo check args
            foreach (var name in names)
            {
                node.AddLinkByName(name);
            }
        }

        public static void DrawLinkFromNodes(this INode node, params INode[] drawFromNodes)
        {
            foreach (var drawFromNode in drawFromNodes)
            {
                drawFromNode.AddLink(node);
            }
        }

        public static void LinkChain(this INode head, params INode[] tail)
        {
            // todo check args

            var current = head;
            if (tail.Any())
            {
                current.AddLink(tail[0]);
            }

            for (var i = 0; i < tail.Length - 1; i++)
            {
                tail[i].AddLink(tail[i + 1]);
            }
        }

        public static T GetLastResult<T>(this IResultAccumulator accumulator)
        {
            // todo checks
            // todo optimize
            return (T)accumulator.Last();
        }
    }
}
