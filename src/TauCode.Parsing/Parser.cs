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

        public object[] ParseOld(INode root, IEnumerable<IToken> tokens)
        {
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            if (tokens == null)
            {
                throw new ArgumentNullException(nameof(tokens));
            }

            var stream = new TokenStream(tokens);
            IContext context = new Context(stream);
            var initialNodes = ParsingHelper.GetNonIdleNodes(new[] { root });

            context.SetNodes(initialNodes);
            var winners = new List<INode>();

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

                winners.Clear();
                var gotActor = false;
                var gotEnd = false;
                var gotSkippers = false;

                foreach (var node in nodes)
                {
                    var inquireResult = node.Inquire(token, context.ResultAccumulator);

                    switch (inquireResult)
                    {
                        case InquireResult.Reject:
                            // bye baby
                            break;

                        case InquireResult.Skip:
                            if (gotActor)
                            {
                                throw new NodeConcurrencyException(
                                    token,
                                    BuildConcurrentNodes(winners, node),
                                    context.ResultAccumulator.ToArray());
                            }
                            gotSkippers = true;
                            winners.Add(node);
                            break;

                        case InquireResult.Act:
                            if (gotActor)
                            {
                                throw new NodeConcurrencyException(
                                    token,
                                    BuildConcurrentNodes(winners, node),
                                    context.ResultAccumulator.ToArray());
                            }
                            gotActor = true;
                            winners.Add(node);
                            break;

                        case InquireResult.End:
                            gotEnd = true;
                            // don't add to winners
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                if (winners.Count == 0)
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
                    else
                    {
                        throw new UnexpectedTokenException(token, context.ResultAccumulator.ToArray());
                    }
                }
                else
                {
                    if (gotActor)
                    {
                        if (winners.Count > 1)
                        {
                            throw new NodeConcurrencyException(
                                token,
                                winners.ToArray(),
                                context.ResultAccumulator.ToArray());
                        }

                        var actor = winners.Single();
                        var oldVersion = context.ResultAccumulator.Version;
                        actor.Act(token, context.ResultAccumulator);
                        if (oldVersion + 1 != context.ResultAccumulator.Version)
                        {
                            throw new InternalParsingLogicException("Internal error. Non sequential result accumulator versions.");
                        }
                    }
                    else
                    {
                        // 'gotSkippers' must be true
                        if (!gotSkippers)
                        {
                            throw new InternalParsingLogicException("Internal parser error.");
                        }
                    }

                    // skip
                    context.TokenStream.AdvanceStreamPosition();
                    var successors = winners.SelectMany(x => x.ResolveLinks()).ToList();

                    var nonIdleSuccessors = ParsingHelper.GetNonIdleNodes(successors);

                    // next nodes
                    context.SetNodes(nonIdleSuccessors);
                }
            }
        }

        private INode[] BuildConcurrentNodes(List<INode> concurrentNodes, INode oneMoreConcurrentNode)
        {
            var allConcurrentNodes = new List<INode>();
            allConcurrentNodes.AddRange(concurrentNodes);
            allConcurrentNodes.Add(oneMoreConcurrentNode);
            return allConcurrentNodes.ToArray();
        }
    }
}
