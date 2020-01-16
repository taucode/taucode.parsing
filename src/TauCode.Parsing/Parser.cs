using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Nodes;

// todo clean
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
            INode winner;
            FallbackNode fallbackNode;
            //var winners = new List<INode>();
            //var fallbackNodes = new List<FallbackNode>();

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

                //winners.Clear();
                //fallbackNodes.Clear();

                winner = null;
                fallbackNode = null;

                //var gotActor = false;
                var gotEnd = false;
                //var gotSkippers = false;

                foreach (var node in nodes)
                {
                    if (node is EndNode)
                    {
                        gotEnd = true;
                        continue;
                    }

                    var inquireResult = node.Inquire(token, context.ResultAccumulator); // todo: rename to 'var acceptsToken'

                    if (inquireResult)
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

                            //fallbackNodes.Add(fallbackNode);
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

                            //if (winners.Count > 0)
                            //{
                            //    winners.Add(node);
                            //    throw new NodeConcurrencyException(token, winners, context.ResultAccumulator.ToArray());
                            //}

                            //winners.Add(node);
                        }
                    }

                    //    switch (inquireResult)
                    //    {
                    //        //case InquireResult.Reject:
                    //        //    // bye baby
                    //        //    break;


                    //        case InquireResult.Skip:
                    //            if (gotActor)
                    //            {
                    //                throw new NodeConcurrencyException(
                    //                    token,
                    //                    BuildConcurrentNodes(winners, node),
                    //                    context.ResultAccumulator.ToArray());
                    //            }
                    //            gotSkippers = true;
                    //            winners.Add(node);
                    //            break;

                    //        case InquireResult.Act:
                    //            if (gotActor)
                    //            {
                    //                throw new NodeConcurrencyException(
                    //                    token,
                    //                    BuildConcurrentNodes(winners, node),
                    //                    context.ResultAccumulator.ToArray());
                    //            }
                    //            gotActor = true;
                    //            winners.Add(node);
                    //            break;

                    //        //case InquireResult.End:
                    //        //    gotEnd = true;
                    //        //    // don't add to winners
                    //        //    break;

                    //        default:
                    //            throw new ArgumentOutOfRangeException();
                    //    }
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



                //if (winners.Count == 0)
                //{
                //    if (gotEnd)
                //    {
                //        if (this.WantsOnlyOneResult)
                //        {
                //            // error. stream has more tokens, but we won't want'em.
                //            throw new UnexpectedTokenException(token, context.ResultAccumulator.ToArray());
                //        }

                //        // fine, got to end, start over.
                //        context.SetNodes(initialNodes);
                //    }
                //    else
                //    {
                //        throw new UnexpectedTokenException(token, context.ResultAccumulator.ToArray());
                //    }
                //}
                //else
                //{
                //    if (gotActor)
                //    {
                //        if (winners.Count > 1)
                //        {
                //            throw new NodeConcurrencyException(
                //                token,
                //                winners.ToArray(),
                //                context.ResultAccumulator.ToArray());
                //        }

                //        var actor = winners.Single();
                //        var oldVersion = context.ResultAccumulator.Version;
                //        actor.Act(token, context.ResultAccumulator);
                //        if (oldVersion + 1 != context.ResultAccumulator.Version)
                //        {
                //            throw new InternalParsingLogicException("Internal error. Non sequential result accumulator versions.");
                //        }
                //    }
                //    else
                //    {
                //        // 'gotSkippers' must be true
                //        if (!gotSkippers)
                //        {
                //            throw new InternalParsingLogicException("Internal parser error.");
                //        }
                //    }

                //    // skip
                //    context.TokenStream.AdvanceStreamPosition();
                //    var successors = winners.SelectMany(x => x.ResolveLinks()).ToList();

                //    var nonIdleSuccessors = ParsingHelper.GetNonIdleNodes(successors);

                //    // next nodes
                //    context.SetNodes(nonIdleSuccessors);
                //}
            }
        }

        //private INode[] BuildConcurrentNodes(List<INode> concurrentNodes, INode oneMoreConcurrentNode)
        //{
        //    var allConcurrentNodes = new List<INode>();
        //    allConcurrentNodes.AddRange(concurrentNodes);
        //    allConcurrentNodes.Add(oneMoreConcurrentNode);
        //    return allConcurrentNodes.ToArray();
        //}
    }
}
