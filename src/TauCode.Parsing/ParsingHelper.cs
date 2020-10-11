using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Nodes;

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
                throw new InvalidOperationException($"'{nameof(tokenStream)}' is at the end. Cannot advance.");
            }

            tokenStream.Position++;
        }

        public static HashSet<INode> GetNonIdleNodes(IReadOnlyCollection<INode> nodes)
        {
            if (nodes.Any(x => x is IdleNode))
            {
                var result = new HashSet<INode>();
                var idleNodes = new HashSet<IdleNode>();

                foreach (var node in nodes)
                {
                    if (node == null)
                    {
                        throw new ArgumentException($"'{nameof(nodes)}' must not contain nulls.");
                    }

                    WriteNonIdleNodes(node, result, idleNodes);
                }

                return result;
            }
            else
            {
                return new HashSet<INode>(nodes);
            }
        }

        private static void WriteNonIdleNodes(INode node, HashSet<INode> destination, HashSet<IdleNode> idleNodes)
        {
            if (node is IdleNode idleNode)
            {
                if (idleNodes.Contains(idleNode))
                {
                    // won't do anything.
                }
                else
                {
                    idleNodes.Add(idleNode);
                    var links = node.ResolveLinks();
                    foreach (var link in links)
                    {
                        WriteNonIdleNodes(link, destination, idleNodes);
                    }
                }
            }
            else
            {
                destination.Add(node);
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
                throw new InvalidOperationException("Result accumulator is empty.");
            }

            var index = accumulator.Count - 1;
            var result = accumulator[index];

            if (result == null)
            {
                throw new NullReferenceException($"Last result is null.");
            }

            if (result.GetType() != typeof(T))
            {
                throw new InvalidCastException(
                    $"Last result expected to be of type '{typeof(T).FullName}', but is of type '{result.GetType().FullName}'.");
            }

            return (T)result;
        }

        public static IReadOnlyCollection<INode> FetchTree(this INode root)
        {
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            var all = new HashSet<INode>();
            var currentNodes = new List<INode>(new[] { root });
            var links = new List<INode>();
            while (true)
            {
                links.Clear();

                foreach (var node in currentNodes)
                {
                    if (!all.Contains(node))
                    {
                        // new one
                        all.Add(node);
                        links.AddRange(node.ResolveLinks());
                    }
                }

                if (links.Count == 0)
                {
                    // nothing to add anymore
                    break;
                }

                currentNodes.Clear();
                currentNodes.AddRange(links);
            }

            return all;
        }

        internal static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            dictionary.TryGetValue(key, out var value);
            return value;
        }
    }
}
