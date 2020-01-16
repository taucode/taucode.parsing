using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Nodes;

namespace TauCode.Parsing
{
    public class Parser : IParser
    {
        public bool WantsOnlyOneResult { get; set; }

        public INode Root { get; set; }

        public object[] Parse(IEnumerable<IToken> tokens)
        {
            var root = this.Root;

            if (root == null)
            {
                throw new NullReferenceException($"Property '{nameof(Root)}' not set.");
            }

            if (tokens == null)
            {
                throw new ArgumentNullException(nameof(tokens));
            }

            var stream = new TokenStream(tokens);
            IParsingContext context = new ParsingContext(stream);
            var initialNodes = ParsingHelper.GetNonIdleNodes(new[] { root });

            context.SetNodes(initialNodes);

            while (true)
            {
                var nodes = context.GetNodes();
                if (stream.IsEndOfStream())
                {
                    if (nodes.Contains(EndNode.Instance))
                    {
                        // met end of stream, but that is acceptable
                        return context.ResultAccumulator.ToArray();
                    }
                    else
                    {
                        throw new UnexpectedEndOfClauseException(context.ResultAccumulator.ToArray());
                    }
                }

                var token = stream.CurrentToken;

                INode winner = null;
                FallbackNode fallbackNode = null;
                var gotEnd = false;

                foreach (var node in nodes)
                {
                    if (node is EndNode)
                    {
                        gotEnd = true;
                        continue;
                    }

                    var acceptsToken = node.AcceptsToken(token, context.ResultAccumulator);

                    if (acceptsToken)
                    {
                        if (node is FallbackNode anotherFallbackNode)
                        {
                            if (fallbackNode != null)
                            {
                                throw new NodeConcurrencyException(
                                    token,
                                    new[] { fallbackNode, anotherFallbackNode },
                                    context.ResultAccumulator.ToArray());
                            }

                            fallbackNode = anotherFallbackNode;
                        }
                        else
                        {
                            if (winner != null)
                            {
                                throw new NodeConcurrencyException(
                                    token,
                                    new []{winner, node},
                                    context.ResultAccumulator.ToArray());
                            }

                            winner = node;
                        }
                    }
                }

                if (winner == null)
                {
                    if (gotEnd)
                    {
                        if (this.WantsOnlyOneResult)
                        {
                            // error. stream has more tokens, but we won't want'em.
                            throw new UnexpectedTokenException(token, context.ResultAccumulator.ToArray());
                        }

                        // fine, got to end, start over.
                        context.SetNodes(initialNodes);
                    }
                    else if (fallbackNode != null)
                    {
                        fallbackNode.Act(token, context.ResultAccumulator); // will throw, and that's what we want.
                    }
                    else
                    {
                        throw new UnexpectedTokenException(token, context.ResultAccumulator.ToArray());
                    }
                }
                else
                {
                    var oldVersion = context.ResultAccumulator.Version;
                    winner.Act(token, context.ResultAccumulator);
                    if (oldVersion + 1 != context.ResultAccumulator.Version)
                    {
                        throw new InternalParsingLogicException("Internal error. Non sequential result accumulator versions.");
                    }

                    // skip
                    context.TokenStream.AdvanceStreamPosition();
                    var successors = winner.ResolveLinks();
                    var nonIdleSuccessors = ParsingHelper.GetNonIdleNodes(successors);

                    // next nodes
                    context.SetNodes(nonIdleSuccessors);
                }
            }
        }
    }
}
