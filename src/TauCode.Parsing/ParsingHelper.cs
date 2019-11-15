using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Nodes;
using TauCode.Utils.CommandLine.Parsing;

namespace TauCode.Parsing
{
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

            if (tokenStream.IsEndOfStream())
            {
                throw new ParsingException($"'{nameof(tokenStream)}' is at the end. Cannot advance.");
            }

            tokenStream.Position++;
        }

        public static IReadOnlyCollection<INode> GetNonIdleNodes(IReadOnlyCollection<INode> nodes)
        {
            if (nodes.Any(x => x is IdleNode))
            {
                var list = new List<INode>();

                foreach (var node in nodes)
                {
                    if (node == null)
                    {
                        throw new ArgumentException($"'{nameof(nodes)}' must not contain nulls.");
                    }

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
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            foreach (var name in names)
            {
                if (name == null)
                {
                    throw new ArgumentException($"'{nameof(names)}' must not contain nulls.");
                }

                node.AddLinkByName(name);
            }
        }

        public static void DrawLinkFromNodes(this INode node, params INode[] drawFromNodes)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            foreach (var drawFromNode in drawFromNodes)
            {
                if (drawFromNode == null)
                {
                    throw new ArgumentException($"'{nameof(drawFromNode)}' must not contain nulls.");
                }

                drawFromNode.AddLink(node);
            }
        }

        public static T GetLastResult<T>(this IResultAccumulator accumulator)
        {
            if (accumulator == null)
            {
                throw new ArgumentNullException(nameof(accumulator));
            }

            if (accumulator.Count == 0)
            {
                throw new ParsingException("Result accumulator is empty.");
            }

            var index = accumulator.Count - 1;
            var result = accumulator[index];

            if (result == null)
            {
                throw new ParsingException($"Last result is null.");
            }

            if (result.GetType() != typeof(T))
            {
                throw new ParsingException($"Last result expected to be of type '{typeof(T).FullName}', but is of type '{result.GetType().FullName}'.");
            }

            return (T)result;
        }
    }
}
